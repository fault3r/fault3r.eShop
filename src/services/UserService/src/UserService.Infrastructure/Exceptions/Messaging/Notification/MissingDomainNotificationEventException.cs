
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Notification;

public class MissingDomainNotificationEventException : InfrastructureException
{
    public MissingDomainNotificationEventException()
        : base("domain notification event is required") { }
}
