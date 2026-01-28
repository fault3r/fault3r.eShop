
using System;
using MediatR;
using Microsoft.Extensions.Hosting;
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

    private static async Task AMomentAsync()
        => await Task.Delay(TimeSpan.FromSeconds(5));

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = await _outbox.DequeueAsync(cancellationToken);

                if (message is null)
                {
                    await AMomentAsync();
                    continue;
                }

                var notification = _mapper.FromNotificationMessage(message!);

                await _mediator.Publish(notification, cancellationToken);
            }
        }
        catch {}
    }
}
 