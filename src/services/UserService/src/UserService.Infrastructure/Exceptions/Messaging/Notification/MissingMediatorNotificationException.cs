
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Notification;

public class MissingMediatorNotificationException : InfrastructureException
{
    public MissingMediatorNotificationException()
        : base("mediator notification is required") { }
}
