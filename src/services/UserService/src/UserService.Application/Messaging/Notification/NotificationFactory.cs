
using System;
using System.Text.Json;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notification.Notifications;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Application.Messaging.Notification;

public sealed class NotificationFactory(
) : INotificationFactory
{
    private readonly JsonSerializerOptions jsonOptions
        = SharedJsonOptions.DefaultOptions;

    public Notification FromEvent(IDomainEvent @event, string correlationId)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        var notification = @event switch
        {
            UserRegisteredEvent e => UserRegisteredNotification.FromEvent(e, correlationId),
            _ => throw new ArgumentException("unsupported event")
        };

        return notification;
    }

    public Notification FromNotificationMessage(NotificationMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var notification = message.Type switch
        {
            nameof(UserRegisteredNotification) => JsonSerializer.Deserialize<UserRegisteredNotification>(message.Payload, jsonOptions),
            _ => throw new ArgumentException("unsupported notification")
        };

        return notification!;
    }
}