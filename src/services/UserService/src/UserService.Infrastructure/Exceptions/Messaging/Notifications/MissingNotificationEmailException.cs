
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Notifications;

public class MissingNotificationEmailException : InfrastructureException
{
    public MissingNotificationEmailException()
        : base("missing notification email") { }
}
