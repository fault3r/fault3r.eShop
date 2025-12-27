
using System;

namespace UserService.Infrastructure.Exceptions.Messaging;

public class MissingMediatorException : InfrastructureException
{
    public MissingMediatorException()
        : base("mediator is required") { }
}
