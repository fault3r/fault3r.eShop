
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Abstractions;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; }
        = DateTime.UtcNow;
}
