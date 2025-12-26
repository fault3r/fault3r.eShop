
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Notification;

public class MissingDomainNotificationException : InfrastructureException
{
    public MissingDomainNotificationException()
        : base("domain notification is required") { }
}
