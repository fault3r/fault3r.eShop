
using System;
using MediatR;
using UserService.Domain.Aggregates.UserAggregate.Events;

namespace UserService.Application.Messaging.Notifications.UserAggregate;

public sealed class UserCreatedNotification : INotification
{
    public string Email { get; }
    public string FullName { get; }

    public UserCreatedNotification(
        string email,
        string fullName)
    {
        Email = email;
        FullName = fullName;
    }

    public static UserCreatedNotification FromEvent(UserCreatedEvent @event)
        => new(@event.Email, @event.FullName);
}
