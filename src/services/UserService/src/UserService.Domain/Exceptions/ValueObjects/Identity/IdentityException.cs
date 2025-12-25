
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public class IdentityException : DomainException
{
    public IdentityException(string message)
        : base(message) { }
}
