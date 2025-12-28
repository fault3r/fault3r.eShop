
using System;

namespace UserService.Domain.Exceptions.PasswordHash;

public sealed class InvalidPasswordHashException : PasswordHashException
{
    public InvalidPasswordHashException(string passwordHash)
        : base($"invalid password hash value: {passwordHash}") { }
}
