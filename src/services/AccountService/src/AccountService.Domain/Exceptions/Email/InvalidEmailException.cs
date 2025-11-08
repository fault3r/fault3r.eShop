using System;

namespace AccountService.Domain.Exceptions.Email;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(string address) : base($"invalid Email address: {address}") { }
}
