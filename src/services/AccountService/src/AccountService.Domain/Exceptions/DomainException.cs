
using System;

namespace AccountService.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException() : base() { }

    public DomainException(string message) : base(message) { }
}

public class MissingFullNameException()
    : DomainException($"FullName is required")
{ }


public class MissingPasswordHashException()
    : DomainException($"PasswordHash is required")
{ }





