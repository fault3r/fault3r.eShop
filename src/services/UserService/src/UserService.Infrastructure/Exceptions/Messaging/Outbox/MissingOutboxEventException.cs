
using System;

namespace UserService.Infrastructure.Exceptions.Messaging.Outbox;

public class MissingOutboxEventException : InfrastructureException
{
    public MissingOutboxEventException()
        : base("outbox domain event is required") { }
}
