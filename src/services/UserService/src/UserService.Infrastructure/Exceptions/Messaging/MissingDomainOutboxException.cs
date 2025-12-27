
using System;

namespace UserService.Infrastructure.Exceptions.Messaging;

public class MissingDomainOutboxException : InfrastructureException
{
    public MissingDomainOutboxException()
        : base("domain outbox is required") { }
}
