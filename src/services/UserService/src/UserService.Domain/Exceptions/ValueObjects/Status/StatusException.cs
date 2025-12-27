
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Status;

public class StatusException : DomainException
{
    public StatusException(string message)
        : base(message) { }
}
