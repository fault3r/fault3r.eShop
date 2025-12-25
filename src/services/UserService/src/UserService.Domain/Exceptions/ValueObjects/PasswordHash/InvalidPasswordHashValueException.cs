
using System;

namespace UserService.Domain.Exceptions.ValueObjects.PasswordHash;

public class InvalidPasswordHashValueException : PasswordHashException
{
    public InvalidPasswordHashValueException(string passwordHash)
        : base($"invalid password hash value: {passwordHash}") { }
}
