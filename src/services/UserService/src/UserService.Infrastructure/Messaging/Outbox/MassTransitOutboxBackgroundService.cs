
using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Application.CrossCutting;
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
                var _serializer = scope.ServiceProvider.GetRequiredService<IJsonSerializer>();

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

                    var messageType = EventTypeResolver.Resolve(message.Type);

                    if (messageType is null)
                    {
                        await _outbox.MarkAsProcessedAsync(message.Id, stoppingToken);

                        _logger.LogInformation("{Correlation} Unknown message type {Type} for message {Id}.",
                            message.CorrelationId, message.Type, message.Id);

                        continue;
                    }

                    object? deserialized;
                    try
                    {
                        deserialized = JsonSerializer.Deserialize(message.Payload, messageType, _serializer.Options);
                    }
                    catch (Exception ex)
                    {
                        await _outbox.MarkAsProcessedAsync(message.Id, stoppingToken);

                        _logger.LogInformation(ex, "{Correlation} Unknown payload for message {Id}.",
                            message.CorrelationId, message.Id);

                        continue;
                    }

                    if (deserialized is null)
                    {
                        await _outbox.MarkAsProcessedAsync(message.Id, stoppingToken);

                        _logger.LogInformation("{Correlation} Null payload for message {Id}.",
                            message.CorrelationId, message.Id);

                        continue;
                    }

                    try
                    {
                        var @event = (IDomainEvent)deserialized;

                        await _publisher.PublishAsync(@event, stoppingToken);

                        await _outbox.MarkAsProcessedAsync(message.Id, stoppingToken);

                        _logger.LogInformation("{Correlation} Message {Id} {Type} published.",
                            message.CorrelationId, message.Id, message.Type);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "{Correlation} Failed to publish message {Id}.",
                            message.CorrelationId, message.Id);
                    }
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Stopping due to cancellation.");

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
