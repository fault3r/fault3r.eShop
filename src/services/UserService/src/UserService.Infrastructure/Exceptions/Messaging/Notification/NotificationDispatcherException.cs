
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Notification;

public class NotificationDispatcherException : InfrastructureException
{
    public NotificationDispatcherException(Exception exception)
        : base("an unexpected error has occurred", exception) { }
}
