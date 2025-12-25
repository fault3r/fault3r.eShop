
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Outbox;

public class MissingOutboxException : InfrastructureException
{
    public MissingOutboxException()
        : base("outbox is required") { }
}
