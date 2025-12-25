
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Status;

public class MissingStatusValueException : StatusException
{
    public MissingStatusValueException()
        : base("status value is required") { }

}
