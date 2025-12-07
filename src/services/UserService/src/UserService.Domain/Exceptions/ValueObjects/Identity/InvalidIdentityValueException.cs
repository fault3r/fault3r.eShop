
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public sealed class InvalidIdentityValueException : DomainException
{
    public InvalidIdentityValueException(string value)
        : base($"invalid identity value: {value}") { }
}
