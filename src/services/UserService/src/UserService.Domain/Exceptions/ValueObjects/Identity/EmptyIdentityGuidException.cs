
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public class EmptyIdentityGuidException : DomainException
{
    public EmptyIdentityGuidException() : base("cannot create Identity with Guid.Empty or null") { }
}
