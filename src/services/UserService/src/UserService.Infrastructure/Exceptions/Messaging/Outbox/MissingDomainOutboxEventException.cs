
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Outbox;

public class MissingDomainOutboxEventException : InfrastructureException
{
    public MissingDomainOutboxEventException()
        : base("domain outbox event is required") { }
}
