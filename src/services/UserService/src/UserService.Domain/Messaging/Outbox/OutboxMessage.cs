
using System;

namespace UserService.Domain.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Payload { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public bool Processed { get; set; }
    public DateTimeOffset ProcessedAt { get; set; }
    public string CorrelationId { get; set; }

    public void MarkAsProcessed()
    {
        Processed = true;
        ProcessedAt = DateTimeOffset.UtcNow;
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