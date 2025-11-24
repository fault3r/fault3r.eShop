
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public class EmptyIdentityValueException : DomainException
{
    public EmptyIdentityValueException() : base("cannot create Identity with null or Guid.Empty") { }
}
