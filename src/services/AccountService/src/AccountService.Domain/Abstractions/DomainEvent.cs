
using System;

namespace AccountService.Domain.Abstractions;

public abstract class DomainEvent
{
    public DateTime OccurredOn { get; protected set; }
        = DateTime.UtcNow;
}
