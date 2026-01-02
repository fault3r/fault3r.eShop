
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging;

public interface IDomainNotification
{
    Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken ct = default
    );
}
