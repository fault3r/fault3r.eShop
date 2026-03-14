
using System;
using MediatR;
using UserService.Application.Messaging.Notification;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class MediatorNotificationDispatcher(
    IMediator mediator
) : INotificationDispatcher
{
    private readonly IMediator _mediator = mediator;

    public async Task DispatchAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        var notification = NotificationMapper.FromEvent(@event, correlationId)
            ?? throw new ArgumentException($"Unsupported event type: {@event.GetType().Name}");

        await _mediator.Publish(notification, cancellationToken);
    }
}