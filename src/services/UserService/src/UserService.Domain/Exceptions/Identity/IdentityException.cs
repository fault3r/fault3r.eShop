
using System;

namespace UserService.Domain.Exceptions.Identity;

public class IdentityException : DomainException
{
    public IdentityException(string message)
        : base(message) { }
}
