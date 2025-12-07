
using System;
using System.Text.Json;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Exceptions.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public class OutboxMessage
{
    public Guid Id { get; init; }
    public DateTime EnqueuedOn { get; init; }
    public string Type { get; init; } 
    public string Payload { get; init; } 

    public OutboxMessage(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new MissingEventException();

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
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
