using System;
using System.Text.Json;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Messaging.Notification;

public class NotificationMessage
{
    public Guid Id { get; init; }
    public DateTime EnqueuedOn { get; init; }
    public required string Type { get; init; }
    public required string Payload { get; init; }
    public required string CorrelationId { get; init; }
}
