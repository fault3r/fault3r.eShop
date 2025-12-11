
using System;
using System.Text.Json;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Exceptions.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public DateTime EnqueuedOn { get; init; }
    public string Type { get; init; } 
    public string Payload { get; init; }    
    public string CorrelationId { get; init; }

    private OutboxMessage(
        IDomainEvent domainEvent, string correlationId)
    {
        if (domainEvent is null)
            throw new MissingEventException();

        Id = domainEvent.EventId;
        EnqueuedOn = domainEvent.OccurredOn;
        Type = domainEvent.GetType().Name;
        Payload = JsonSerializer.Serialize(
            domainEvent, domainEvent.GetType(), jsonSerializerOptions);
        CorrelationId = correlationId;
    }

    public static OutboxMessage FromDomainEvent(
        IDomainEvent domainEvent,  string correlationId)
            => new(domainEvent, correlationId);

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
