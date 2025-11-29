
using System;

namespace UserService.Domain.Exceptions.Common.Result;

public class MissingResultValueException : DomainException
{
    public MissingResultValueException() : base("result value is required") { }
}
