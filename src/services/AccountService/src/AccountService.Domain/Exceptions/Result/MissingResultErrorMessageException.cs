
using System;

namespace AccountService.Domain.Exceptions.Result;

public class MissingResultErrorMessageException : DomainException
{
    public MissingResultErrorMessageException() : base("Result error message is required"){}
}