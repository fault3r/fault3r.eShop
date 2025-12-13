
using System;
using System.Text.Json;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Exceptions.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; }
    public DateTime EnqueuedOn { get; private set; }
    public string CorrelationId { get; private set; }

    private OutboxMessage(
        IDomainEvent domainEvent,
        string correlationId)
    {
        if (domainEvent is null)
            throw new MissingEventException();

        Id = domainEvent.EventId;
        Type = domainEvent.GetType().Name;
        Payload = JsonSerializer.Serialize(
            domainEvent, domainEvent.GetType(), jsonSerializerOptions);
        EnqueuedOn = domainEvent.OccurredOn;
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

    // EFCore
    private OutboxMessage() { }  
}
