
using System;
using FluentEmail.Core;
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

    private static async Task SomeSecondsAsync(int second = 5)
        => await Task.Delay(TimeSpan.FromSeconds(second));

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
                var messages = await _outbox.DequeueAsync(cancellationToken);

                if (!messages.Any()) continue;

                var notifications = messages.Select(_factory.FromNotificationMessage);

                foreach(var notification in notifications)
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

            finally { await SomeSecondsAsync(); }
        }
    }

}
