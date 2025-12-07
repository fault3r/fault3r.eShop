
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class PersistenceException : InfrastructureException
{
    public PersistenceException(Exception innerException) 
        : base("error committing transaction", innerException) { }
}
