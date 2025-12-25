
using System;

namespace UserService.Domain.Exceptions.ValueObjects.FullName;

public class MissingFullNameValueException : FullNameException
{
    public MissingFullNameValueException()
        : base("fullname value is required") { }
}
