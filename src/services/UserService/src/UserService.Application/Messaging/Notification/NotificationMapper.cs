
using System;
using System.Text.Json;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notification.Notifications;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Application.Messaging.Notification;

public sealed class NotificationMapper : INotificationMapper
{
    public INotification FromEvent(IDomainEvent @event, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return @event switch
        {
            UserRegisteredEvent e => UserRegisteredNotification.FromEvent(e, correlationId),
            _ => throw new Exception()
        };
    }

    public INotification FromNotificationMessage(NotificationMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var notification = message.Type switch
        {
            nameof(UserRegisteredNotification) =>
                JsonSerializer.Deserialize<UserRegisteredNotification>(message.Payload, jsonSerializerOptions),
            _ => throw new Exception()
        };

        return notification!;
    }
    
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
