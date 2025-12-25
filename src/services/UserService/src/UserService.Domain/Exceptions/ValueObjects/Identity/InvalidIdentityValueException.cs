
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public class InvalidIdentityValueException : IdentityException
{
    public InvalidIdentityValueException(string identity)
        : base($"invalid identity value: {identity}") { }
}
