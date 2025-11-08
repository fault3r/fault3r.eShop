using System;

namespace AccountService.Domain.Exceptions.Email;

public class MissingEmailException : DomainException
{
    public MissingEmailException() : base("Email address is required"){}
}
