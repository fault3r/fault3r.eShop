
using System;
using System.Text.Json;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notification.Notifications;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Application.Messaging.Notification;

public sealed class NotificationMapper(
    JsonSerializerOptions jsonSerializerOptions
) : INotificationMapper
{
    private readonly JsonSerializerOptions _jsonOptions = jsonSerializerOptions;

    public INotification FromEvent(IDomainEvent @event, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var notification = @event switch
        {
            UserRegisteredEvent e => UserRegisteredNotification.FromEvent(e, correlationId),
            _ => throw new ArgumentException("unsupported event")
        };

        return notification;
    }

    public INotification FromNotificationMessage(NotificationMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var notification = message.Type switch
        {
            nameof(UserRegisteredNotification) => JsonSerializer.Deserialize<UserRegisteredNotification>(message.Payload, _jsonOptions),
            _ => throw new ArgumentException("unsupported notification")
        };

        return notification!;
    }
}