
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Email;

public sealed class EmptyEmailAddressException : DomainException
{
    public EmptyEmailAddressException() : base("Email address is required") { }
}
