
using System;

namespace UserService.Domain.Messaging.Notification;

public sealed record NotificationMessage
{
    public Guid Id { get; init; }
    public DateTime EnqueuedOn { get; init; }
    public required string Type { get; init; }
    public required string Payload { get; init; }
}
