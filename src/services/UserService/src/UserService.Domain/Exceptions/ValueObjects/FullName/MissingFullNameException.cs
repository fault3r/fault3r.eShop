
using System;

namespace UserService.Domain.Exceptions.ValueObjects.FullName;

public class MissingFullNameException : DomainException
{
    public MissingFullNameException() : base("fullname is required") { }
}
