
using System;
using AccountService.Domain.Abstractions;

namespace AccountService.Domain.Services;

public interface IOutboxMessageService
{
    Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}
