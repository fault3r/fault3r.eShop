
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class RepositoryArgumentException : InfrastructureException
{
    public RepositoryArgumentException()
        : base("repository argument is required") { }
}
