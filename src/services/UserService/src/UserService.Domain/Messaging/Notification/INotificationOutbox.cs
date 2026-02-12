
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

    Task<IEnumerable<NotificationMessage>> DequeueAsync(
        CancellationToken cancellationToken = default
    );
}
