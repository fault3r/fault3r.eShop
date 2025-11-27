
using System;

namespace UserService.Domain.Exceptions.Common.Result;

public class MissingResultValueException : DomainException
{
    public MissingResultValueException() : base("Result value is required") { }
}
