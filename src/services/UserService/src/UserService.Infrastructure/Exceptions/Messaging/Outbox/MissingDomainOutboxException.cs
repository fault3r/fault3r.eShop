
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Outbox;

public class MissingDomainOutboxException : InfrastructureException
{
    public MissingDomainOutboxException()
        : base("domain outbox is required") { }
}
