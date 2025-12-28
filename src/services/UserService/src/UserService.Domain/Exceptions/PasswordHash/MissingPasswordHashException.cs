
using System;

namespace UserService.Domain.Exceptions.PasswordHash;

public sealed class MissingPasswordHashException : PasswordHashException
{
    public MissingPasswordHashException()
        : base($"password hash is required") { }
}
