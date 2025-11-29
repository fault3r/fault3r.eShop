
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public sealed class EmptyIdentityValueException : DomainException
{
    public EmptyIdentityValueException() : base("cannot create identity with null or Guid.Empty") { }
}
