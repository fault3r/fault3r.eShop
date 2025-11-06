
using System;
using AccountService.Domain.Common;

namespace AccountService.Domain.Base;

public abstract class DomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public required Identity AggregateId { get; init; }
}
