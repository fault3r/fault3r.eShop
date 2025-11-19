
using System;

namespace AccountService.Domain.Exceptions.Result;

public class MissingErrorMessageException : DomainException
{
    public MissingErrorMessageException() : base("result Error message is required"){}
}