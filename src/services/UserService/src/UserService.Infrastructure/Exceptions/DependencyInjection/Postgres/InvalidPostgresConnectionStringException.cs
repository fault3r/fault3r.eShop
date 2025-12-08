
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection.Postgres;
public class InvalidPostgresConnectionStringException : InfrastructureException
{
    public InvalidPostgresConnectionStringException()
        : base("invalid postgres connection string") { }
}
