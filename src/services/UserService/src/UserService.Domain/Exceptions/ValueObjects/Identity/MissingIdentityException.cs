
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public sealed class MissingIdentityException : DomainException
{
    public MissingIdentityException() : base("identity is required") { }
}
