
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Email;

public sealed class MissingEmailAddressException : DomainException
{
    public MissingEmailAddressException() : base("email address is required") { }
}
