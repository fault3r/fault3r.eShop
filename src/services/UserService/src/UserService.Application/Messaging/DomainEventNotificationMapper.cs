
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notifications.UserAggregate;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;

namespace UserService.Application.Messaging;

public sealed class DomainEventNotificationMapper : IEventNotificationMapper
{
    public INotification Map(IDomainEvent @event)
    {
        // should i check for???? -> if(@event is null) 
        // throw new CustomException(); 
        // or throw new ArgumentNullException()
        return @event switch
        {
            UserCreatedEvent e => UserCreatedNotification.FromEvent(e),
            _ => throw new Exception()
        };
    }

}
