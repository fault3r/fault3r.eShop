
using MediatR;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Infrastructure.Exceptions.Messaging.Notifications;

namespace UserService.Infrastructure.Messaging.DomainEventDispatcher.Notifications;

public sealed class UserCreatedNotification : INotification
{
    public string Email { get; }
    public string FullName { get; }

    public UserCreatedNotification(
        string email,
        string fullName)
    {
        Email = email
            ?? throw new MissingNotificationEmailException();

        FullName = fullName
            ?? throw new MissingNotificationFullNameException();
    }

    public static UserCreatedNotification FromDomainEvent(
        UserCreatedEvent @event)
    {
        if (@event is null)
            throw new MissingNotificationEventException();

        return new(@event.Email, @event.FullName);
    }
}
