
using System;
using MediatR;
using UserService.Domain.Aggregates.UserAggregate.Events;

namespace UserService.Application.Messaging.Notification.Notifications;

public sealed class UserRegisteredNotification : BaseNotification
{
    public string Email { get; init; }
    public string FullName { get; init; }

    public UserRegisteredNotification(
        string email,
        string fullName,
        string correlationId
    ) : base(correlationId)
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

        return new(@event.Email, @event.FullName, correlationId);
    }
}
