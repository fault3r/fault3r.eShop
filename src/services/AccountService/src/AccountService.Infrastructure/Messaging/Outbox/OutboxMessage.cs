
using System;
using System.Text.Json;
using AccountService.Domain.Exceptions.DomainEvent;
using AccountService.Domain.Interfaces;

namespace AccountService.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public int Id { get; set; }
    public DateTime EnqueuedOn { get; init; }
    public string Type { get; init; } = default!;
    public string Payload { get; init; } = default!;

    public OutboxMessage(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new MissingDomainEventException();
            
        EnqueuedOn = domainEvent.OccurredOn;
        Type = domainEvent.GetType().Name;
        Payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType());
    }

    public static OutboxMessage FromDomainEvent(IDomainEvent domainEvent)
        => new(domainEvent);

    private OutboxMessage() { } //for EF Core
}
