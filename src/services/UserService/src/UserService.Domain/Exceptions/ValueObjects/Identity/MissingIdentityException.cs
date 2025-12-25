
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public class MissingIdentityException : IdentityException
{
    public MissingIdentityException()
        : base("identity is required") { }
}
