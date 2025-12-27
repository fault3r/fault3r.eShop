
using System;

namespace UserService.Domain.Exceptions.Email;

public class EmailException : DomainException
{
    public EmailException(string message)
        : base(message) { }
}
