using System;

namespace AccountService.Domain.Exceptions.Status;

public class UnsupportedStatusException : DomainException
{
    public UnsupportedStatusException(string value) : base($"unsupported Status value: {value}") { }
}
