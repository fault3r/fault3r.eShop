
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging;

public interface INotificationOutbox
{
    Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default
    );
}
