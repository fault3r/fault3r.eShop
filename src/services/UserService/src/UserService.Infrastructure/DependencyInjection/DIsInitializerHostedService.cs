
using System;
using Microsoft.Extensions.Hosting;
using UserService.Infrastructure.Messaging.Notification;

namespace UserService.Infrastructure.DependencyInjection;

public sealed class DIsInitializerHostedService(
    RedisNotificationOutbox redisNotificationOutbox
) : IHostedService
{
    private readonly RedisNotificationOutbox _notificationOutbox = redisNotificationOutbox;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _notificationOutbox.Initialize();
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
