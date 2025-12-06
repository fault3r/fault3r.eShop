
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class MissingEventsException : InfrastructureException
{
    public MissingEventsException() : base("domain events is required") { }
}
