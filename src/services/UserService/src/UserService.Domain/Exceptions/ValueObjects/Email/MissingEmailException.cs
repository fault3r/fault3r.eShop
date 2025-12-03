
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Email;

public sealed class MissingEmailException : DomainException
{
    public MissingEmailException() : base("email is required") { }
}
