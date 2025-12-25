
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Email;

public sealed class InvalidEmailValueException : EmailException
{
    public InvalidEmailValueException(string email)
        : base($"invalid email value: {email}") { }
}
