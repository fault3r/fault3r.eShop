
using System;

namespace AccountService.Domain.Exceptions.FullName;

public class InvalidFullNameException : DomainException
{
    public InvalidFullNameException(string name) : base($"invalid FullName: {name}") { }
}