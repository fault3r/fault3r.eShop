
using MediatR;
using UserService.Domain.Aggregates.UserAggregate.Events;

namespace UserService.Application.Messaging.Notifications;

public sealed class UserCreatedNotification : INotification
{
    public string Email { get; }
    public string FullName { get; }

    public UserCreatedNotification(
        string email,
        string fullName)
    {
        Email = email
            ?? throw new ArgumentNullException(nameof(email));

        FullName = fullName
            ?? throw new ArgumentNullException(nameof(fullName));
    }

    public static UserCreatedNotification FromDomainEvent(
        UserCreatedEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return new(@event.Email, @event.FullName);
    }
}
