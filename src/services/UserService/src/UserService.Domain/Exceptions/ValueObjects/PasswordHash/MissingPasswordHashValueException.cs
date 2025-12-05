
using System;

namespace UserService.Domain.Exceptions.ValueObjects.PasswordHash;

public class MissingPasswordHashValueException : DomainException
{
    public MissingPasswordHashValueException() : base($"password hash is required") { }
}
