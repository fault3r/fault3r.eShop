
using System;

namespace AccountService.Domain.Exceptions.FullName;

public class MissingFullNameException : DomainException
{
    public MissingFullNameException() : base("FullName is required") { }
}