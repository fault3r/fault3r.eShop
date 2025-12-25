
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Email;

public sealed class MissingEmailException : EmailException
{
    public MissingEmailException()
        : base("email is required") { }
}
