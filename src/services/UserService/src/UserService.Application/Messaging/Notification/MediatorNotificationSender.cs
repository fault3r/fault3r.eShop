
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notification.Notifications;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;

namespace UserService.Application.Messaging.Notification;

public sealed class MediatorNotificationSender : INotificationSender
{
    private readonly IMediator _mediator;
    private readonly Dictionary<Type, Func<IDomainEvent, string, NotificationMessage>> _notificationMappers;

    public MediatorNotificationSender(IMediator mediator)
    {
        _mediator = mediator;

        _notificationMappers = new()
        {
            { typeof(UserRegisteredEvent), (e, corrId) => UserRegisteredNotification.FromEvent((UserRegisteredEvent)e, corrId) },
        };
    }

    public async Task PublishAsync(
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