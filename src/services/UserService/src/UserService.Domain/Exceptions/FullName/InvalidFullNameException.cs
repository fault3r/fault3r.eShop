
using System;

namespace UserService.Domain.Exceptions.FullName;

public class InvalidFullNameException : FullNameException
{
    public InvalidFullNameException(string fullName)
        : base($"invalid fullname value: {fullName}") { }
}
