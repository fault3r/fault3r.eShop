
using System;
using System.Text.Json;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Exceptions.CrossCutting;
using UserService.Infrastructure.Exceptions.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public DateTime EnqueuedOn { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; }
    public string CorrelationId { get; private set; }

    private OutboxMessage(
        IDomainEvent domainEvent,
        string correlationId)
    {
        if (domainEvent is null)
            throw new MissingOutboxEventException();

        if (string.IsNullOrWhiteSpace(correlationId))
            throw new MissingCorrelationIdException();

        Id = domainEvent.EventId;
        EnqueuedOn = domainEvent.OccurredOn;
        Type = domainEvent.GetType().Name;
        Payload = JsonSerializer.Serialize(
            domainEvent, domainEvent.GetType(), jsonSerializerOptions);
        CorrelationId = correlationId;
    }

    public static OutboxMessage FromEvent(
        IDomainEvent domainEvent,
        string correlationId)
    {
        return new(domainEvent, correlationId);
    }

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    // EFCore
    public OutboxMessage() { }
}
