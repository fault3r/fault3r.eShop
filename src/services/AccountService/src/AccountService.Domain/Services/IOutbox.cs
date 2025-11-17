
using System;
using System.Collections.ObjectModel;
using AccountService.Domain.Interfaces;

namespace AccountService.Domain.Services;

public interface IOutbox
{
    Task DispatchAsync(ReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
