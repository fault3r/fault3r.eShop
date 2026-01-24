
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notifications;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;

namespace UserService.Application.Messaging;

public sealed class EventNotificationMapper : IEventNotificationMapper
{
    public INotification ToNotification(IDomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return @event switch
        {
            UserRegisteredEvent e => UserRegisteredNotification.FromEvent(e),
            _ => throw new InvalidOperationException("unsupported domain event")
        };
    }
}
