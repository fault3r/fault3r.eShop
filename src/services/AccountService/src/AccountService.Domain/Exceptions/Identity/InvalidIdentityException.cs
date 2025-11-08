using System;

namespace AccountService.Domain.Exceptions.Identity;

public class InvalidIdentityException : DomainException
{
    public InvalidIdentityException(string value) : base($"invalid Identity value: {value}") { }
}
