
using System;
using System.Text.Json;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; }
    public bool Processed { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public string CorrelationId { get; private set; }

    public OutboxMessage(
        IDomainEvent domainEvent,
        string correlationId)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        ArgumentException.ThrowIfNullOrEmpty(correlationId);

        Id = domainEvent.EventId;
        Type = domainEvent.GetType().Name;
        Payload = JsonSerializer.Serialize(
            domainEvent, domainEvent.GetType(), SharedJsonOptions.DefaultOptions);
        Processed = false;
        Timestamp = domainEvent.OccurredOn;
        CorrelationId = correlationId;
    }

    public static OutboxMessage FromEvent(
        IDomainEvent domainEvent,
        string correlationId)
    {
        return new(domainEvent, correlationId);
    }

    public void MarkAsProcessed() => Processed = true;
    
    #region ⤚EFCore
    public OutboxMessage()
    {
        Type = null!;
        Payload = null!;
        CorrelationId = null!;
    }
    #endregion
}