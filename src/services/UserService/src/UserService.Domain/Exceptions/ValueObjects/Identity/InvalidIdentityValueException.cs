
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public class InvalidIdentityValueException : DomainException
{
    public InvalidIdentityValueException(string value) : base($"invalid Identity value: {value}") { }
}
