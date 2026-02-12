
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging.Notification;

public interface INotificationOutbox
{
    Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken = default
    );

    Task<NotificationMessage?> DequeueAsync(
        CancellationToken cancellationToken = default
    );

    Task MarkAsFailureAsync(
        NotificationMessage message,
        CancellationToken cancellationToken = default
    );

    Task MarkAsProcessedAsync(
        NotificationMessage message,
        CancellationToken cancellationToken = default
    );
}
