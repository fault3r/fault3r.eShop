
using System;

namespace UserService.Domain.Exceptions.Status;

public sealed class UnsupportedStatusException : StatusException
{
    public UnsupportedStatusException(string value)
        : base($"unsupported status: {value}") { }
}
