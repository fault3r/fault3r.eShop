
using System;

namespace UserService.Infrastructure.Exceptions.CrossCutting;

public class MissingCorrelationIdException : InfrastructureException
{
    public MissingCorrelationIdException()
        : base("correlation id is required") { }
}