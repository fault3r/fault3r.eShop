
using System;

namespace UserService.Domain.Exceptions.PasswordHash;

public class MissingPasswordHashException : PasswordHashException
{
    public MissingPasswordHashException()
        : base($"password hash is required") { }
}
