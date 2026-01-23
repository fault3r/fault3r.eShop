
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging;

public interface INotificationOutbox
{
    Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default
    );
}
