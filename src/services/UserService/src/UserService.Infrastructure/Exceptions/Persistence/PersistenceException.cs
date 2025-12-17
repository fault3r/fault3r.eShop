
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class PersistenceException : InfrastructureException
{
    public PersistenceException() 
        : base("unexpected persistence error") { }
}
