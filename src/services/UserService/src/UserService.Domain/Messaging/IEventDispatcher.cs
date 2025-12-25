
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging;

public interface IEventDispatcher
{
    Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default
    );
}
