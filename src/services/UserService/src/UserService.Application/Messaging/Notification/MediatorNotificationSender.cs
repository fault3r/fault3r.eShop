
using System;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Messaging.Notification;

public sealed class MediatorNotificationSender(
    IMediator mediator
) : INotificationSender
{
    private readonly IMediator _mediator = mediator;

    public async Task PublishAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        var notification = NotificationMapper.Map(@event, correlationId)
            ?? throw new ArgumentException($"Unsupported event type: {@event.GetType().Name}");

        await _mediator.Publish(notification, cancellationToken);
    }
}