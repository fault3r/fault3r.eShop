
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Status;

public sealed class MissingStatusValueException : DomainException
{
    public MissingStatusValueException()
        : base("status value is required") { }

}
