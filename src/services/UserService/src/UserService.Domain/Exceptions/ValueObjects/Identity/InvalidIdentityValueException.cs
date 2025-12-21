
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public sealed class InvalidIdentityValueException : DomainException
{
    public InvalidIdentityValueException(string identity)
        : base($"invalid identity value: {identity}") { }
}
