
using System;

namespace UserService.Domain.Exceptions.ValueObjects.FullName;

public class InvalidFullNameValueException : FullNameException
{
    public InvalidFullNameValueException(string fullName)
        : base($"invalid fullname value: {fullName}") { }
}
