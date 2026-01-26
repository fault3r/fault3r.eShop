
using System;
using MediatR;

namespace UserService.Application.Messaging.Notification;

public abstract class BaseNotification : INotification
{
    public string CorrelationId { get; init; }

    public BaseNotification(string correlationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        CorrelationId = correlationId;
    }
}
