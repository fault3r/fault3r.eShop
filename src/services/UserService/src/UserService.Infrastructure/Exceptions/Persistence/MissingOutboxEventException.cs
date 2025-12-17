
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class MissingOutboxEventException : InfrastructureException
{
    public MissingOutboxEventException()
        : base("outbox domain event is required") { }
}
