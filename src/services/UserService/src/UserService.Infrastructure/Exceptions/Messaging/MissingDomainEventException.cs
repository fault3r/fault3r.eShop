
using System;

namespace UserService.Infrastructure.Exceptions.Messaging;

public class MissingDomainEventException : InfrastructureException
{
    public MissingDomainEventException()
        : base("domain event is required") { }
}
