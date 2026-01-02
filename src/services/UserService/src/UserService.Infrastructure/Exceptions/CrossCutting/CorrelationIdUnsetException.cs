
using System;

namespace UserService.Infrastructure.Exceptions.CrossCutting;

public sealed class CorrelationIdUnsetException : InfrastructureException
{
    public CorrelationIdUnsetException()
        : base("correlation id has not been set") { }
}
