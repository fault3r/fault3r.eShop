
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;
public class PostgresConnectionException : InfrastructureException
{
    public PostgresConnectionException()
        : base("can not connect to postgres") { }
}
