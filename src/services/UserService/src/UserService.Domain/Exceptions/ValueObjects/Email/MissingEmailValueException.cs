
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Email;

public sealed class MissingEmailValueException : EmailException
{
    public MissingEmailValueException()
        : base("email value is required") { }
}
