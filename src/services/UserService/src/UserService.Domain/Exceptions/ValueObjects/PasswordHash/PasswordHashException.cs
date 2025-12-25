
using System;

namespace UserService.Domain.Exceptions.ValueObjects.PasswordHash;

public class PasswordHashException : DomainException
{
    public PasswordHashException(string message)
        : base(message) { }
}