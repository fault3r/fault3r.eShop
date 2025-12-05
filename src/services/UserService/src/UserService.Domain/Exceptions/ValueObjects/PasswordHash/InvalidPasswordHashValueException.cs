
using System;

namespace UserService.Domain.Exceptions.ValueObjects.PasswordHash;

public class InvalidPasswordHashValueException : DomainException
{
    public InvalidPasswordHashValueException(string passwordHash) : base($"invalid password hash value: {passwordHash}") { }
}
