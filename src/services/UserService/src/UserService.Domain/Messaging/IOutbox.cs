
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging;

public interface IOutbox
{
    Task EnqueueAsync(
        IEnumerable<IDomainEvent> domainEvents,
        string correlationId,
        CancellationToken cancellationToken = default
    );
}
