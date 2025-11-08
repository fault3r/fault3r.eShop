using System;

namespace AccountService.Domain.Exceptions.Identity;

public class EmptyGuidException : DomainException
{
    public EmptyGuidException() : base("cannot create Identity with Guid.Empty") { }
}
