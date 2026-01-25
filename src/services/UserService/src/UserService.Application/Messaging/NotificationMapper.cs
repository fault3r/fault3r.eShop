
using System;
using System.Text.Json;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notifications;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Application.Messaging;

public sealed class NotificationMapper : INotificationMapper
{
    public INotification FromEvent(IDomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return @event switch
        {
            UserRegisteredEvent e => UserRegisteredNotification.FromEvent(e),
            _ => throw new Exception()
        };
    }

    public INotification FromNotificationMessage(NotificationMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);


        var res = message.Type switch
        {
            nameof(UserRegisteredNotification) =>
                JsonSerializer.Deserialize<UserRegisteredNotification>(message.Payload),
            _ => throw new Exception()
        };

        return res!;
    }
    
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
