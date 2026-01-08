
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class MediatorDomainNotification(
    IMediator mediator,
    IEventNotificationMapper mapper
) : IDomainNotification
{
    private readonly IMediator _mediator = mediator;
    private readonly IEventNotificationMapper _mapper = mapper;

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(events);

        if (events.Any(e => e is null))
            throw new ArgumentException($"{nameof(events)} contains null element");

        foreach (var @event in events)
        {
            var notification = _mapper.ToNotification(@event);
            
            await _mediator.Publish(notification, cancellationToken);
        }
    }
}

