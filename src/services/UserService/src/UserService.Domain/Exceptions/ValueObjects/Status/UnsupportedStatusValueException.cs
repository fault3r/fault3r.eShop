
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Status;

public sealed class UnsupportedStatusValueException : DomainException
{
    public UnsupportedStatusValueException(string value)
        : base($"invalid status value: {value}") { }
}
