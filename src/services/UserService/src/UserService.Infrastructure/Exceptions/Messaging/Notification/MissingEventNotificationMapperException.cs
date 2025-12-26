
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Notification;

public class MissingEventNotificationMapperException : InfrastructureException
{
    public MissingEventNotificationMapperException()
        : base("event-notification mapper is required") { }
}
