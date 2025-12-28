
using System;

namespace UserService.Infrastructure.Exceptions;

public class InfrastructureException : Exception
{
    public InfrastructureException(string message)
        : base(message) { }
}
