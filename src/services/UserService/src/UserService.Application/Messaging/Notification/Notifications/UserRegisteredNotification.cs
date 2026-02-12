
using System;
using UserService.Domain.Aggregates.UserAggregate.Events;

namespace UserService.Application.Messaging.Notification.Notifications;

public sealed class UserRegisteredNotification : Notification
{
    public string Email { get; init; }
    public string FullName { get; init; }

    public UserRegisteredNotification(
        string userId,
        string email,
        string fullName,
        DateTimeOffset timestamp,
        string correlationId
    ) : base(userId, timestamp, correlationId)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        ArgumentException.ThrowIfNullOrEmpty(fullName);

        Email = email;
        FullName = fullName;
    }

    public static UserRegisteredNotification FromEvent(
        UserRegisteredEvent @event,
        string correlationId)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        return new(@event.UserId, @event.Email, @event.FullName, @event.OccurredOn, correlationId);
    }
}
