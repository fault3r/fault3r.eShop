
using System;

namespace UserService.Domain.Exceptions.Status;

public sealed class MissingStatusException : StatusException
{
    public MissingStatusException()
        : base("status is required") { }

}
