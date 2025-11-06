
using System;
using AccountService.Domain.Common;

namespace AccountService.Domain.Abstractions;

public abstract class DomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
