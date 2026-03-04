
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notification.Notifications;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;

namespace UserService.Application.Messaging.Notification;

public sealed class MediatorNotificationSender(
    IMediator mediator
) : INotificationSender
{
    private readonly IMediator _mediator = mediator;

    public async Task SendAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        var notification = @event switch
        {
            UserRegisteredEvent e => UserRegisteredNotification.FromEvent(e, correlationId),
            _ => throw new ArgumentException("unsupported event")
        };

        await _mediator.Publish(notification, cancellationToken);
    }
}