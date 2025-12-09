
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection.Postgres;
public class PostgresConnectionException : InfrastructureException
{
    public PostgresConnectionException()
        : base("can not connect to postgres") { }
}
