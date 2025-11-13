
using System;

namespace AccountService.Infrastructure.Exceptions.Persistence;

public class UnitOfWorkException : InfrastructureException
{
    public UnitOfWorkException() : base() { }

    public UnitOfWorkException(string message) : base(message) { }
}
