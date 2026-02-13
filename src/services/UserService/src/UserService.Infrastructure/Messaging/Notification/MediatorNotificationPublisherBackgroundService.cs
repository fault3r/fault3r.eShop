
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
        await using var scope = _scopeFactory.CreateAsyncScope();
        var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var _outbox = scope.ServiceProvider.GetRequiredService<INotificationOutbox>();
        var _factory = scope.ServiceProvider.GetRequiredService<INotificationFactory>();

        var logger = Log.ForContext<MediatorNotificationPublisherBackgroundService>();

        while (!cancellationToken.IsCancellationRequested)
        {
            NotificationMessage? message = null;
            try
            {
                message = await _outbox.DequeueAsync(cancellationToken);

                if (message is null) continue;

                var notification = _factory.FromNotificationMessage(message);

                logger.Information("{Correlation} Publishing {Type}…", message.CorrelationId, notification.GetType().Name);

                await _mediator.Publish(notification, cancellationToken);

                await _outbox.MarkAsProcessedAsync(message, cancellationToken);

                logger.Information("{Correlation} {Type} published.", message.CorrelationId, notification.GetType().Name);
            }

            catch (StackExchange.Redis.RedisConnectionException)
            {
                logger.Error("Redis connection error, Reconnectiong…");
            }
            catch (Exception ex)
            {
                await _outbox.MarkAsFailureAsync(message!, cancellationToken);

                logger.Error("Failed to publish notification, {Error}", ex.Message);
            }

            finally
            {
                await SomeSecondsAsync(second: 5, cancellationToken);
            }
        }
    }

    private static async Task SomeSecondsAsync(
        int second = 5,
        CancellationToken cancellationToken = default)
    => await Task.Delay(TimeSpan.FromSeconds(second), cancellationToken);
}
