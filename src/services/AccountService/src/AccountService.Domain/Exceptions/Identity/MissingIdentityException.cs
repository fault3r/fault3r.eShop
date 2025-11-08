using System;

namespace AccountService.Domain.Exceptions.Identity;

public class MissingIdentityException : DomainException
{
    public MissingIdentityException() : base("Identity value is required") { }
}