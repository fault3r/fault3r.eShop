
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Email;

public sealed class InvalidEmailAddressException : DomainException
{
    public InvalidEmailAddressException(string adress) : base($"invalid Email address: {adress}") { }
}
