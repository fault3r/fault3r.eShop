
using System;
using MediatR;

namespace UserService.Application.Messaging.Notification;

public abstract class Notification : INotification
{
    public string UserId { get; init; }
    public string CorrelationId { get; init; }

    public Notification(string userId, string correlationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        UserId = userId;
        CorrelationId = correlationId;
    }
}
