using System;

namespace AccountService.Domain.Exceptions.Identity;

public class EmptyIdentityException : DomainException
{
    public EmptyIdentityException() : base("cannot create Identity with Guid.Empty") { }
}
