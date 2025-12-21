
using System;

namespace UserService.Domain.Exceptions.ValueObjects.FullName;

public class InvalidFullNameException : DomainException
{
    public InvalidFullNameException(string fullName)
        : base($"invalid fullname: {fullName}") { }
}
