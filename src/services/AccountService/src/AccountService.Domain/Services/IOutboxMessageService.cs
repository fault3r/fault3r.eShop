
using System;
using AccountService.Domain.Abstractions;

namespace AccountService.Domain.Services;

public interface IOutboxMessageService
{
    Task EnqueueAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}
