
using System;

namespace UserService.Domain.Exceptions.Status;

public class MissingStatusException : StatusException
{
    public MissingStatusException()
        : base("status is required") { }

}
