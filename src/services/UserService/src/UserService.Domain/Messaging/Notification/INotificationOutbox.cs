
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging.Notification;

public interface INotificationOutbox
{
    Task EnqueueAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken
    );

    Task<NotificationMessage?> DequeueAsync(
        CancellationToken cancellationToken
    );

    Task MarkAsProcessedAsync(
        NotificationMessage notification,
        CancellationToken cancellationToken
    );
}
