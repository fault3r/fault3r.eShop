
using System;

namespace AccountService.Infrastructure.Exceptions.Persistence;

public class RepositoryException : InfrastructureException
{
    public RepositoryException() : base() { }

    public RepositoryException(string message) : base(message) { }
}
