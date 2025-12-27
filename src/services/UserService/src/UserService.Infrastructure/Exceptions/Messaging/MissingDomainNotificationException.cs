
using System;

namespace UserService.Infrastructure.Exceptions.Messaging;

public class MissingDomainNotificationException : InfrastructureException
{
    public MissingDomainNotificationException()
        : base("domain notification is required") { }
}
