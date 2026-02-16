
using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using UserService.Application.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class MediatorNotificationPublisherBackgroundService(
    IServiceScopeFactory serviceScopeFactory
) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var logger = Log.ForContext<MediatorNotificationPublisherBackgroundService>();

        while (!cancellationToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var _outbox = scope.ServiceProvider.GetRequiredService<INotificationOutbox>();
            var _factory = scope.ServiceProvider.GetRequiredService<INotificationFactory>();

            NotificationMessage? message = null;
            string correlationId = string.Empty;
            try
            {
                message = await _outbox.DequeueAsync(cancellationToken);

                if (message is null)
                {
                    await Task.Delay(3000, cancellationToken);
                    continue;
                }

                logger.Information("{Correlation} Publishing {Type}…", message.CorrelationId, message.Type);

                correlationId = message.CorrelationId;

                var notification = _factory.FromNotificationMessage(message);

                await _mediator.Publish(notification, cancellationToken);

                await _outbox.MarkAsProcessedAsync(message, cancellationToken);

                logger.Information("{Correlation} {Type} published.", message.CorrelationId, message.Type);
            }

            catch (StackExchange.Redis.RedisConnectionException)
            {
                logger.Error("Redis connection error, Reconnectiong…");
                await Task.Delay(3000, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error("{Correlation} Failed to publish notification, {Error}", correlationId, ex.Message);
                await Task.Delay(3000, cancellationToken);
            }
        }
    }
}
