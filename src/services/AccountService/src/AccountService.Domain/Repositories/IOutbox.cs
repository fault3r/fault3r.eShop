
using System;
using System.Collections.ObjectModel;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Interfaces;

namespace AccountService.Domain.Repositories;

public interface IOutbox
{
    Task EnqueueAsync(ReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
