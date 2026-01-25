
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging.Notification;

public interface INotificationOutbox
{
    Task EnqueueAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default
    );

    Task<NotificationMessage?> DequeueAsync(CancellationToken cancellationToken = default);
}
