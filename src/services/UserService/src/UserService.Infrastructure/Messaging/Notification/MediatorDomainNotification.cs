
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging;
using UserService.Infrastructure.Exceptions.Messaging.Notification;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class MediatorDomainNotification : IDomainNotification
{
    private readonly IMediator _mediator;
    private readonly IDomainEventNotificationMapper _mapper;

    public MediatorDomainNotification(
        IMediator mediator,
        IDomainEventNotificationMapper mapper)
    {
        _mediator = mediator
            ?? throw new MissingMediatorNotificationException();

        _mapper = mapper
            ?? throw new MissingEventNotificationMapperException();
    }

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var @event in events)
            {
                var notification = _mapper.Map(@event);
                await _mediator.Publish(notification, cancellationToken);
            }
        }
        catch { throw new NotificationDispatcherException(); }
    }
}

