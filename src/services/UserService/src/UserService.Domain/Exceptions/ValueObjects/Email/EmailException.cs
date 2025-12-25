
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Email;

public class EmailException : DomainException
{
    public EmailException(string message)
        : base(message) { }
}
