
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class MissingDbContextException : InfrastructureException
{
    public MissingDbContextException() : base("database context is required") { }
}
