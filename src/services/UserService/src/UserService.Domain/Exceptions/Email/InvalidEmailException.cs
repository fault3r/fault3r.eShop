
using System;

namespace UserService.Domain.Exceptions.Email;

public sealed class InvalidEmailException : EmailException
{
    public InvalidEmailException(string message)
        : base(message) { }
}
