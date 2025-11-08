
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

public class MissingEmailException()
    : DomainException($"Email address is required")
{ }

public class MissingPasswordHashException()
    : DomainException($"PasswordHash is required")
{ }

public class MissingRoleException()
    : DomainException($"Role is required")
{ }

public class InvalidEmailException(string address)
    : DomainException($"invalid Email address: {address}")
{ }

public class UnsupportedRoleException(string name)
    : DomainException($"unsupported Role: {name}")
{ }

public class MissingStatusException()
    : DomainException($"Status value is required")
{ }

public class UnsupportedStatusException(string value)
    : DomainException($"unsupported Status value: {value}")
{ }