
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class MissingQueryExpressionException : InfrastructureException
{
    public MissingQueryExpressionException()
        : base("query expression is required") { }
}
