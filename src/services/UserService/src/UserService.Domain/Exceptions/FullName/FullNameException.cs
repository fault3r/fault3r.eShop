
using System;

namespace UserService.Domain.Exceptions.FullName;

public class FullNameException : DomainException
{
    public FullNameException(string message)
        : base(message) { }
}
