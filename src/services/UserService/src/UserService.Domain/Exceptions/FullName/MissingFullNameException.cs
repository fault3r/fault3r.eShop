
using System;

namespace UserService.Domain.Exceptions.FullName;

public class MissingFullNameException : FullNameException
{
    public MissingFullNameException()
        : base("fullname is required") { }
}


