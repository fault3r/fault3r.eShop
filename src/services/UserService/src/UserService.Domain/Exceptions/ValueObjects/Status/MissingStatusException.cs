
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Status;

public sealed class MissingStatusException : DomainException
{
    public MissingStatusException()
        : base("status is required") { }

}
