
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Abstractions;

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public DateTime OccurredOn { get; init; }

    protected DomainEvent(Guid? eventId = null, DateTime? occurredOn = null)
    {
        EventId = eventId
            ?? Guid.NewGuid();

        OccurredOn = occurredOn
            ?? DateTime.UtcNow;
    }

    public override string ToString()
        => $"[{OccurredOn:O}] {GetType().Name} (EventId={EventId})";
}
