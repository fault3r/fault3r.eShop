
using System;

namespace UserService.Domain.Exceptions.FullName;

public sealed class MissingFullNameException : FullNameException
{
    public MissingFullNameException()
        : base("fullname is required") { }
}


