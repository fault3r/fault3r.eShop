

using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Notifications;

public class MissingNotificationFullNameException : InfrastructureException
{
    public MissingNotificationFullNameException()
        : base("missing notification full name") { }
}
