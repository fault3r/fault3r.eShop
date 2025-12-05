
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Outbox;

public interface IOutbox
{
    Task EnqueueAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
