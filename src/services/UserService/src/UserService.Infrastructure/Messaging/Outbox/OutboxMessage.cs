
using System;
using System.Text.Json;
using UserService.Domain.Exceptions.Abstraction.AggregateRoot;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Messaging.Outbox;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public DateTime EnqueuedOn { get; init; }
    public string Type { get; init; } = default!;
    public string Payload { get; init; } = default!;

    public OutboxMessage(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new MissingDomainEventException();

        Id = domainEvent.EventId;
        EnqueuedOn = domainEvent.OccurredOn;
        Type = domainEvent.GetType().Name;
        Payload = JsonSerializer.Serialize(
            domainEvent, domainEvent.GetType(), jsonSerializerOptions);
    }

    public static OutboxMessage FromDomainEvent(IDomainEvent domainEvent)
        => new(domainEvent);

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
        WriteIndented = false,
    };
}
