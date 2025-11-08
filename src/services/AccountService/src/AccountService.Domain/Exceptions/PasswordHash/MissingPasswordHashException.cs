
using System;

namespace AccountService.Domain.Exceptions.PasswordHash;

public class MissingPasswordHashException : DomainException
{
    public MissingPasswordHashException() : base($"PasswordHash is required"){ }
}
