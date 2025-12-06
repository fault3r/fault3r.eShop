
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class InvalidEventException : InfrastructureException
{
    public InvalidEventException() : base("domain event is required") { }
}
