
using System;
using System.Text.Json;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public DateTime EnqueuedOn { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; }
    public bool Published { get; private set; }
    public string CorrelationId { get; private set; }

    public OutboxMessage(
        IDomainEvent domainEvent,
        string correlationId)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        ArgumentException.ThrowIfNullOrEmpty(correlationId);

        Id = domainEvent.EventId;
        EnqueuedOn = domainEvent.OccurredOn;
        Type = domainEvent.GetType().Name;
        Payload = JsonSerializer.Serialize(
            domainEvent, domainEvent.GetType(), SharedJsonOptions.DefaultOptions);
        Published = false;
        CorrelationId = correlationId;
    }

    public static OutboxMessage FromEvent(
        IDomainEvent domainEvent,
        string correlationId)
    {
        return new(domainEvent, correlationId);
    }

    #region ⤚EFCore
    public OutboxMessage()
    {
        Type = null!;
        Payload = null!;
        CorrelationId = null!;
    }
    #endregion
}