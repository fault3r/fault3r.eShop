using System;

namespace AccountService.Domain.Exceptions.Identity;

public class MissingGuidException : DomainException
{
    public MissingGuidException() : base("Guid is required") { }
}