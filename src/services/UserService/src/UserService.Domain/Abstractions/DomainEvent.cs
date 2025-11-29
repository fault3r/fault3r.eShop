
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Abstractions;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; }
        = DateTime.UtcNow;
}
