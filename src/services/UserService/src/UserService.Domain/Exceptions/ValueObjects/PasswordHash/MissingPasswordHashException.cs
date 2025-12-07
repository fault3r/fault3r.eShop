
using System;

namespace UserService.Domain.Exceptions.ValueObjects.PasswordHash;

public class MissingPasswordHashException : DomainException
{
    public MissingPasswordHashException()
        : base($"password hash is required") { }
}
