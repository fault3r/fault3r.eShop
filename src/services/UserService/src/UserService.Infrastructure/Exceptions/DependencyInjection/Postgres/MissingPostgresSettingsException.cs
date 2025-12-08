
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection.Postgres;

public class MissingPostgresSettingsException : InfrastructureException
{
    public MissingPostgresSettingsException()
        : base("missing postgres settings") { }
}
