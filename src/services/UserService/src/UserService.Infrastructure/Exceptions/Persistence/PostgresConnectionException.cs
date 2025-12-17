
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;
public class PostgresConnectionException : InfrastructureException
{
    public PostgresConnectionException()
        : base("cannot connect to postgres server") { }
}
