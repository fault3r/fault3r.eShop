
using System;
using UserService.Application.Messaging.Notification;
using UserService.Application.Messaging.Notification.Notifications;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Messaging.Notification;

public static class NotificationMapper
{
    private static readonly Dictionary<Type, Func<IDomainEvent, string, NotificationMessage>> _mappers = new()
    {
        { typeof(UserRegisteredEvent), (e, corrId) => UserRegisteredNotification.FromEvent((UserRegisteredEvent)e, corrId) },
    };

    public static NotificationMessage? FromEvent(IDomainEvent @event, string correlationId)
    {        
        if (_mappers.TryGetValue(@event.GetType(), out var mapper))
            return mapper.Invoke(@event, correlationId);

        return null;
    }
}
