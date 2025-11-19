
using System;

namespace AccountService.Domain.Exceptions.Result;

public class MissingValueException : DomainException
{
    public MissingValueException() : base("result Value is required"){}
}