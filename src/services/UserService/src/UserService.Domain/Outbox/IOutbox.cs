
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Outbox;

public interface IOutbox
{
    Task SaveAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
