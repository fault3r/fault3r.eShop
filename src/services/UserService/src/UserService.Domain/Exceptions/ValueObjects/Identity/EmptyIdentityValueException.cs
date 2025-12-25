
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public class EmptyIdentityValueException : IdentityException
{
    public EmptyIdentityValueException()
        : base("cannot create identity with null or Guid.Empty") { }
}
