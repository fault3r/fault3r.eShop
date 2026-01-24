
using System;

namespace UserService.Domain.Messaging.Notification;

public interface INotificationOutbox
{
    Task EnqueueAsync(
        NotificationMessage notification,
        string correlationId,
        CancellationToken cancellationToken = default
    );
}
