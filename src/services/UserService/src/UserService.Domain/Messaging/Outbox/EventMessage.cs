
using System;

namespace UserService.Domain.Messaging.Outbox;

public sealed class EventMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Payload { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public bool Published { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public string CorrelationId { get; set; }

    public void MarkAsPublished()
    {
        Published = true;
        PublishedAt = DateTimeOffset.UtcNow;
    }
    
    #region ⤚EFCore
    public EventMessage()
    {
        Type = null!;
        Payload = null!;
        CorrelationId = null!;
    }
    #endregion
}