
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Outbox;

public interface IOutbox
{
    Task EnqueueAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
