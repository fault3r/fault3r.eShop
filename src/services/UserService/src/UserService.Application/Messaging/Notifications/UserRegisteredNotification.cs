
using System;
using MediatR;
using UserService.Domain.Aggregates.UserAggregate.Events;

namespace UserService.Application.Messaging.Notifications;

public sealed record UserRegisteredNotification : INotification
{
    public string Email { get; init; }
    public string FullName { get; init; }

    public UserRegisteredNotification(
        string email,
        string fullName)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        ArgumentException.ThrowIfNullOrEmpty(fullName);

        Email = email;
        FullName = fullName;
    }

    public static UserRegisteredNotification FromEvent(UserRegisteredEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return new(@event.Email, @event.FullName);
    }
}
