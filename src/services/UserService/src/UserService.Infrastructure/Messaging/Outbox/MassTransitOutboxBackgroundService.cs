
using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Outbox;
using UserService.Infrastructure.Messaging.Bus;

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
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _provider.CreateScope();
                var _outbox = scope.ServiceProvider.GetRequiredService<IEventOutbox>();
                var _publisher = scope.ServiceProvider.GetRequiredService<IMessageBus>();

                var messages = await _outbox.DequeueAsync(
                    count: 5,
                    cancellationToken: stoppingToken
                );

                if (!messages.Any())
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
                        await _outbox.MarkAsProcessedAsync(message.Id, stoppingToken);

                        _logger.LogWarning("Unknown message type: {Type} for OutboxMessage {Id}", message.Type, message.Id);

                        continue;
                    }

                    object? payload;
                    try
                    {
                        payload = JsonSerializer.Deserialize(message.Payload, messageType, SharedJsonSerializer.DefaultOptions);
                    }
                    catch (Exception ex)
                    {
                        await _outbox.MarkAsProcessedAsync(message.Id, stoppingToken);

                        _logger.LogWarning(ex, "Failed to deserialize payload for OutboxMessage {Id}.", message.Id);

                        continue;
                    }

                    IDomainEvent @event;
                    if (payload is null)
                    {
                        await _outbox.MarkAsProcessedAsync(message.Id, stoppingToken);

                        _logger.LogWarning("Null payload for OutboxMessage {Id}.", message.Id);

                        continue;
                    }
                    @event = (IDomainEvent)payload;

                    try
                    {
                        await _publisher.PublishAsync(@event, stoppingToken);

                        await _outbox.MarkAsProcessedAsync(message.Id, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "MassTransit failed to publish OutboxMessage {Id} of type {Type}", message.Id, message.Type);
                    }
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Outbox Dispatcher is stopping due to cancellation.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occurred!");
            }

            await Task.Delay(500, stoppingToken);
        }
    }
}
