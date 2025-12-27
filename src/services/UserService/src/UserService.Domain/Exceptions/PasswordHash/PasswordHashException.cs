
using System;

namespace UserService.Domain.Exceptions.PasswordHash;

public class PasswordHashException : DomainException
{
    public PasswordHashException(string message)
        : base(message) { }
}