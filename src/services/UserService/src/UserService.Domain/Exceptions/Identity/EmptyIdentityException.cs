
using System;

namespace UserService.Domain.Exceptions.Identity;

public class EmptyIdentityException : IdentityException
{
    public EmptyIdentityException()
        : base("cannot create identity with null or Guid.Empty") { }
}
