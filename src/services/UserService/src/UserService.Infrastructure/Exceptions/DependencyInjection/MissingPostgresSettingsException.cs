
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingPostgresSettingsException : InfrastructureException
{
    public MissingPostgresSettingsException()
        : base("missing postgreSQL database settings") { }
}
