
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Abstractions;

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; init; }
    public DateTime OccurredOn { get; init; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;        
    }

    protected DomainEvent(Guid eventId, DateTime occurredOn)
    {
        EventId = eventId;
        OccurredOn = occurredOn;
    }

    public override string ToString()
        => $"[{OccurredOn:O}] {GetType().Name} (EventId={EventId})";}