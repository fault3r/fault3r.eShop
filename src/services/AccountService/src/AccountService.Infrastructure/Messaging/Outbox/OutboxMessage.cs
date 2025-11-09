
using System;
using AccountService.Domain.Abstractions;

namespace AccountService.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public int Id { get; init; }
    public DateTime EnqueuedOn { get; init; }
    public string Type { get; init; } = default!;
    public string Payload { get; init; } = default!;

    private OutboxMessage(DomainEvent @event)
    {
        Id = @event.

    }

    public static OutboxMessage FromDomainEvent(DomainEvent @event)
        => new()
}
