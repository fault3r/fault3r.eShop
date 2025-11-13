
using System;

namespace AccountService.Infrastructure.Exceptions.Persistence;

public class OutBoxException : InfrastructureException
{
    public OutBoxException() : base() { }
    
    public OutBoxException(string message) : base(message) { }
}