using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Infrastructure.Persistence.Contexts;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class MassTransitOutboxBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<MassTransitOutboxBackgroundService> logger
) : BackgroundService
{
    private readonly IServiceProvider _provider = serviceProvider;
    private readonly ILogger<MassTransitOutboxBackgroundService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Dispatcher started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _provider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
                var publisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                var messages = await dbContext.OutboxMessages
                    .Where(p => !p.Processed)
                    .OrderBy(p => p.Timestamp)
                    .Take(5)
                    .ToListAsync(stoppingToken);

                if (messages.Count == 0)
                {
                    await Task.Delay(500, stoppingToken);
                    continue;
                }

                foreach (var message in messages)
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    var messageType = OutboxTypeResolver.Resolve(message.Type);
                    if (messageType == null)
                    {
                        message.MarkAsProcessed();
                        message.SetProcessedAt(DateTimeOffset.UtcNow);

                        _logger.LogWarning(
                            "Unknown message type: {Type} for OutboxMessage {Id}",
                            message.Type,
                            message.Id
                        );

                        continue;
                    }

                    object? body;
                    try
                    {
                        body = JsonSerializer.Deserialize(message.Payload, messageType);
                    }
                    catch (Exception ex)
                    {
                        message.MarkAsProcessed();
                        message.SetProcessedAt(DateTimeOffset.UtcNow);

                        _logger.LogError(
                            ex,
                            "Failed to deserialize payload for OutboxMessage {Id} of type {Type}",
                            message.Id,
                            message.Type
                        );

                        continue;
                    }

                    if (body is null)
                    {
                        message.MarkAsProcessed();
                        message.SetProcessedAt(DateTimeOffset.UtcNow);

                        _logger.LogError(
                            "Deserialized payload is null for OutboxMessage {Id} of type {Type}",
                            message.Id,
                            message.Type
                        );

                        continue;
                    }

                    try
                    {
                        await publisher.Publish(body, messageType, stoppingToken);

                        message.MarkAsProcessed();
                        message.SetProcessedAt(DateTimeOffset.UtcNow);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "MassTransit failed to publish OutboxMessage {Id} of type {Type}",
                            message.Id,
                            message.Type
                        );
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Outbox Dispatcher is stopping due to cancellation.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox Dispatcher loop failed unexpectedly.");
            }

            await Task.Delay(500, stoppingToken);
        }

        _logger.LogInformation("Outbox Dispatcher stopped");
    }
}
