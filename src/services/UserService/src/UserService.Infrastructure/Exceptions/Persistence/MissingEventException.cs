
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class MissingEventException : InfrastructureException
{
    public MissingEventException()
        : base("domain event is required") { }
}
