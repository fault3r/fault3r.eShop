
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Notifications;

public class MissingNotificationEventException : InfrastructureException
{
    public MissingNotificationEventException()
        : base("missing notification event") { }
}
