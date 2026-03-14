
using System;
using MediatR;

namespace UserService.Application.Messaging.Notification;

public abstract class NotificationMessage : INotification
{
    public string UserId { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public string CorrelationId { get; init; }

    protected NotificationMessage(
        string userId,
        DateTimeOffset timestamp,
        string correlationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        UserId = userId;
        Timestamp = timestamp;
        CorrelationId = correlationId;
    }
}
