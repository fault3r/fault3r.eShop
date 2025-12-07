
using System;

namespace UserService.Domain.Exceptions.Common.Result;

public class MissingResultErrorMessageException : DomainException
{
    public MissingResultErrorMessageException()
        : base("result error message is required") { }
}
