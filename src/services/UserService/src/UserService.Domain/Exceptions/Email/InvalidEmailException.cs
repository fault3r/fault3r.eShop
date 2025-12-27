
using System;

namespace UserService.Domain.Exceptions.Email;

public class InvalidEmailException : EmailException
{
    public InvalidEmailException(string message)
        : base(message) { }
}
