
using System;
using MediatR;

namespace UserService.Application.Messaging.Notification;

public abstract class UserNotification : INotification
{
    public string UserId { get; init; }
    public string CorrelationId { get; init; }

    public UserNotification(string userId, string correlationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        UserId = userId;
        CorrelationId = correlationId;
    }
}
