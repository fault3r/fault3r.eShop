
using System;

namespace UserService.Domain.Exceptions.Identity;

public sealed class MissingIdentityException : IdentityException
{
    public MissingIdentityException()
        : base("identity is required") { }
}
