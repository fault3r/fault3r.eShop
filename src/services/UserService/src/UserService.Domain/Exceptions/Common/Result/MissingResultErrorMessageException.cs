
using System;

namespace UserService.Domain.Exceptions.Common.Result;

public class MissingResultErrorMessageException : DomainException
{
    public MissingResultErrorMessageException() : base("Result error message is required") { }
}
