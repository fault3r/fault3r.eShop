
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Outbox;

public interface IOutbox
{
    Task StoreAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
