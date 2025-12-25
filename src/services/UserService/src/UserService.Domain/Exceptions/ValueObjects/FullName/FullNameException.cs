
using System;

namespace UserService.Domain.Exceptions.ValueObjects.FullName;

public class FullNameException : DomainException
{
    public FullNameException(string message)
        : base(message) { }
}
