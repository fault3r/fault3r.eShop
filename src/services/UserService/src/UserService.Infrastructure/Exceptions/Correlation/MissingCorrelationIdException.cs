
using System;

namespace UserService.Infrastructure.Exceptions.Correlation;

public class MissingCorrelationIdException : InfrastructureException
{
    public MissingCorrelationIdException() : base("correlation id is required") { }
}