
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging;

public interface INotificationOutbox
{
    Task EnqueueAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default
    );
}
