
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public sealed class EmptyIdentityValueException : DomainException
{
    public EmptyIdentityValueException() : base("cannot create Identity with null or Guid.Empty") { }
}
