
using System;

namespace AccountService.Domain.Abstractions;

public abstract class DomainEvent
{
    protected DateTime OccurredOn { get; }
        = DateTime.UtcNow;
}
