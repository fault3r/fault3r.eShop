
using System;

namespace UserService.Domain.Exceptions.Identity;

public class InvalidIdentityException : IdentityException
{
    public InvalidIdentityException(string identity)
        : base($"invalid identity value: {identity}") { }
}
