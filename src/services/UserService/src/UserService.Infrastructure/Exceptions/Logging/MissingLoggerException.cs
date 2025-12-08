
using System;

namespace UserService.Infrastructure.Exceptions.Logging;

public class MissingLoggerException : InfrastructureException
{
    public MissingLoggerException()
        : base("missing logger") { }
}
