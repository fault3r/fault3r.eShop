
using System;

namespace UserService.Domain.Messaging.Notification;

public sealed record NotificationMessage
{
    public Guid Id { get; init; }
    public required string Type { get; init; }
    public required string Payload { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public required string CacheId { get; init; }
    public required string CorrelationId { get; init; }
}
