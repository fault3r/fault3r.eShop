
using System;

namespace UserService.Domain.Exceptions.Abstraction.Entity;

public class MissingEntityIdentityException : DomainException
{
    public MissingEntityIdentityException() : base("Entity identity is required") { }
}
