
using System;
using AccountService.Infrastructure.Exceptions;

namespace AccountService.Infrastructure.Exceptions.Persistence;

public class DbContextException : InfrastructureException
{
    public DbContextException() : base() { }

    public DbContextException(string message) : base(message) { }
}