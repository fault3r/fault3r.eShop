
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Status;

public class MissingStatusException : StatusException
{
    public MissingStatusException()
        : base("status is required") { }

}
