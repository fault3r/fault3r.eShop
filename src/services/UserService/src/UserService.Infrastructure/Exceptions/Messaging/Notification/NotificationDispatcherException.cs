
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Notification;

public class NotificationDispatcherException : InfrastructureException
{
    public NotificationDispatcherException()
        : base("an unexpected error occurred") { }
}
