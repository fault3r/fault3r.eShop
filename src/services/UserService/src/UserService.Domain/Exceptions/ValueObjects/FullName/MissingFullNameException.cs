
using System;

namespace UserService.Domain.Exceptions.ValueObjects.FullName;

public class MissingFullNameException : FullNameException
{
    public MissingFullNameException()
        : base("fullname is required") { }
}


