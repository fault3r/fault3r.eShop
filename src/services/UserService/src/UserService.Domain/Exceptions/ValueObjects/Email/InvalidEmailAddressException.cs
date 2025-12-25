
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Email;

public sealed class InvalidEmailAddressException : EmailException
{
    public InvalidEmailAddressException(string email)
        : base($"invalid email address: {email}") { }
}
