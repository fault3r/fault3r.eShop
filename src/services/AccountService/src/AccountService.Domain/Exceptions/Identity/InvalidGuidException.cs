using System;

namespace AccountService.Domain.Exceptions.Identity;

public class InvalidGuidException : DomainException
{
    public InvalidGuidException(string guid) : base($"invalid Guid: {guid}") { }
}
