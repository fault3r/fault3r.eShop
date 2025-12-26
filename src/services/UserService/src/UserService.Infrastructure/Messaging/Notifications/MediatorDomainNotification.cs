
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging;

namespace UserService.Infrastructure.Messaging.Notifications;

public sealed class MediatorDomainNotification : IDomainNotification
{
    private readonly IMediator _mediator;
    private readonly IDomainEventNotificationMapper _mapper;

    public MediatorDomainNotification(
        IMediator mediator,
        IDomainEventNotificationMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
            var notification = _mapper.Map(@event);
            await _mediator.Publish(notification, cancellationToken);
        }
    }
}

