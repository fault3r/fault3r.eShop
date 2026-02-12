
using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StackExchange.Redis;
using UserService.Application.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class MediatorNotificationPublisherBackgroundService(
    IServiceScopeFactory serviceScopeFactory
) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

    private static async Task SomeSecondsAsync(int second = 5, CancellationToken cancellationToken = default)
        => await Task.Delay(TimeSpan.FromSeconds(second), cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var _outbox = scope.ServiceProvider.GetRequiredService<INotificationOutbox>();
        var _factory = scope.ServiceProvider.GetRequiredService<INotificationFactory>();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = await _outbox.DequeueAsync(cancellationToken);

                if (message is null) continue;

                var notification = _factory.FromNotificationMessage(message);

                await _mediator.Publish(notification, cancellationToken);
            }

            catch (RedisConnectionException)
            {
                Log.Error(nameof(MediatorNotificationPublisherBackgroundService)
                    + " Failed to connect to the Redis database, Reconnectiong…");
            }
            catch (Exception ex)
            {
                Log.Error(ex, nameof(MediatorNotificationPublisherBackgroundService)
                    + " An unhandled exception occurred!");
            }

            finally { await SomeSecondsAsync(second: 5, cancellationToken); }
        }
    }

}
