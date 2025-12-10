
using System;

namespace UserService.Infrastructure.Exceptions.Common;

public class MissingCorrelationIdException : InfrastructureException
{
    public MissingCorrelationIdException() : base("correlation id is required") { }
}