
using System;
using UserService.Domain.Aggregates.UserAggregate.Events;

namespace UserService.Application.Messaging.Notification.Notifications;

public sealed class UserRegisteredNotification : UserNotification
{
    public string Email { get; init; }
    public string FullName { get; init; }

    public UserRegisteredNotification(
        string userId,
        string email,
        string fullName,
        string correlationId
    ) : base(userId, correlationId)
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

        return new(@event.UserId, @event.Email, @event.FullName, correlationId);
    }
}
