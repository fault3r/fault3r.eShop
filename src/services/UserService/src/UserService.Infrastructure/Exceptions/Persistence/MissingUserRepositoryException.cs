
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class MissingUserRepositoryException : InfrastructureException
{
    public MissingUserRepositoryException()
        : base("user repository is required") { }
}
