
using System;
using MediatR;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging;
using UserService.Infrastructure.Messaging.DomainEventDispatcher.Notifications;

namespace UserService.Infrastructure.Messaging.DomainEventDispatcher;

public sealed class MediatorDomainEventDispatcher : IEventDispatcher
{
    private readonly IMediator _mediator;

    public MediatorDomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in events)
        {            
            var notification = domainEvent switch
            {
                UserCreatedEvent e => UserCreatedNotification.FromDomainEvent(e),
                _ => null
            };
            if (notification is not null)
                await _mediator.Publish(notification, cancellationToken);
        }
    }
}

