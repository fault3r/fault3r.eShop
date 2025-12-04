
using System;

namespace UserService.Domain.Exceptions.ValueObjects.FullName;

public class InvalidFullNameException : DomainException
{
    public InvalidFullNameException(string fullName) : base($"full name must be between 2 and 99 characters, invalid fullname: {fullName}") { }
}
