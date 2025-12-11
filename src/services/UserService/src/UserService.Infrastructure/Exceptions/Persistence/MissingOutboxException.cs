
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class MissingOutboxException : InfrastructureException
{
    public MissingOutboxException()
        : base("outbox is required") { }
}
