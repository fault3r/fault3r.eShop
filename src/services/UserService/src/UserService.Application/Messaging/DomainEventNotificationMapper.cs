
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notifications.UserAggregate;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;

namespace UserService.Application.Messaging;

public sealed class DomainEventNotificationMapper : IDomainEventNotificationMapper
{
    public INotification Map(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            UserCreatedEvent e => UserCreatedNotification.FromEvent(e),
            _ => throw new Exception()
        };
    }

}
