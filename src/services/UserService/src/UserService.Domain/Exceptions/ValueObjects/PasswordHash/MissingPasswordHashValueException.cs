
using System;

namespace UserService.Domain.Exceptions.ValueObjects.PasswordHash;

public class MissingPasswordHashValueException : PasswordHashException
{
    public MissingPasswordHashValueException()
        : base($"password hash value is required") { }
}
