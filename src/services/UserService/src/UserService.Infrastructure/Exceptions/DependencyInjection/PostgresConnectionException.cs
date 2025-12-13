
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;
public class PostgresConnectionException : InfrastructureException
{
    public PostgresConnectionException()
        : base("cannot connect to postgres server") { }
}
