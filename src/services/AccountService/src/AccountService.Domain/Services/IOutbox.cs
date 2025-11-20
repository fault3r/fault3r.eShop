
using System;
using System.Collections.ObjectModel;
using AccountService.Domain.Interfaces;

namespace AccountService.Domain.Services;

public interface IOutbox
{
    Task EnqueueAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
