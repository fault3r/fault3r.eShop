
using System;
using AccountService.Domain.Abstractions;

namespace AccountService.Domain.Repositories;

public interface IOutbox
{
    Task EnqueueAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);

    Task EnqueueRangeAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
