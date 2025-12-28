
using System;

namespace UserService.Infrastructure.Exceptions.CrossCutting;

public class CorrelationIdUnsetException : InfrastructureException
{
    public CorrelationIdUnsetException()
        : base("correlation id has not been set") { }
}