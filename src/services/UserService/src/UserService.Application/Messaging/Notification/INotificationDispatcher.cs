
using System;
using UserService.Domain.Interfaces;

namespace UserService.Application.Messaging.Notification;

public interface INotificationDispatcher
{
    Task DispatchAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default
    );
}
