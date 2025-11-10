
using System;
using System.Text.Json;
using AccountService.Domain.Interfaces;

namespace AccountService.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public DateTime EnqueuedOn { get; init; }
    public string Type { get; init; } = default!;
    public string Payload { get; init; } = default!;

    public static OutboxMessage FromDomainEvent(IDomainEvent domainEvent)
        => new()
        {
            EnqueuedOn = domainEvent.OccurredOn,
            Type = domainEvent.GetType().Name,
            Payload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
        };

    private OutboxMessage() { }
}
