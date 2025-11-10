
using System;
using AccountService.Domain.Interfaces;

namespace AccountService.Domain.Abstractions;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; protected set; }
        = Guid.NewGuid();

    public DateTime OccurredOn { get; protected set; }
        = DateTime.UtcNow;
}
