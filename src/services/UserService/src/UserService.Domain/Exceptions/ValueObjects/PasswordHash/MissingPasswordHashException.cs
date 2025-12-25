
using System;

namespace UserService.Domain.Exceptions.ValueObjects.PasswordHash;

public class MissingPasswordHashException : PasswordHashException
{
    public MissingPasswordHashException()
        : base($"password hash is required") { }
}
