
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging;
using UserService.Infrastructure.Exceptions.Messaging;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class MediatorDomainNotification : IDomainNotification
{
    private readonly IMediator _mediator;
    private readonly IEventNotificationMapper _mapper;

    public MediatorDomainNotification(
        IMediator mediator,
        IEventNotificationMapper mapper)
    {
        _mediator = mediator
            ?? throw new MissingMediatorException();

        _mapper = mapper
            ?? throw new MissingEventNotificationMapperException();
    }

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        if (events is null)
            throw new MissingDomainEventException();

        foreach (var @event in events)
        {
            var notification = _mapper.Map(@event);
            await _mediator.Publish(notification, cancellationToken);
        }
    }
}

