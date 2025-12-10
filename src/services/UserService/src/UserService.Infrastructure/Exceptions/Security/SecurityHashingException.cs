
using System;

namespace UserService.Infrastructure.Exceptions.Security;

public class SecurityHashingException : InfrastructureException
{
    public SecurityHashingException(string message) : base(message) { }
}
