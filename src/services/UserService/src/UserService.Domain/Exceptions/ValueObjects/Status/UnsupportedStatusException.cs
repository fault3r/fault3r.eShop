
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Status;

public class UnsupportedStatusException : StatusException
{
    public UnsupportedStatusException(string value)
        : base($"unsupported status: {value}") { }
}
