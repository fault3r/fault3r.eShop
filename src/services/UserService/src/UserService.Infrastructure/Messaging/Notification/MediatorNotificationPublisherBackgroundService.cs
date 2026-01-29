
using System;
using MediatR;
using Microsoft.Extensions.Hosting;
using Serilog;
using StackExchange.Redis;
using UserService.Application.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class MediatorNotificationPublisherBackgroundService(
    IMediator mediator,
    INotificationOutbox outbox,
    INotificationMapper mapper
) : BackgroundService
{
    private readonly IMediator _mediator = mediator;
    private readonly INotificationOutbox _outbox = outbox;
    private readonly INotificationMapper _mapper = mapper;

    private static async Task SomeSecondsAsync(int second = 5)
        => await Task.Delay(TimeSpan.FromSeconds(second));

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = await _outbox.DequeueAsync(cancellationToken);

                if (message is null) continue; 

                var notification = _mapper.FromNotificationMessage(message!);

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
