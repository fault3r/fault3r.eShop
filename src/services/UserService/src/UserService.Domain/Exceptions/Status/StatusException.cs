
using System;

namespace UserService.Domain.Exceptions.Status;

public class StatusException : DomainException
{
    public StatusException(string message)
        : base(message) { }
}
