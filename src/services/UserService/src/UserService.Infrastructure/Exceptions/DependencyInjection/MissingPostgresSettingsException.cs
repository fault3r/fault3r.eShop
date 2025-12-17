
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingPostgresSettingsException : InfrastructureException
{
    public MissingPostgresSettingsException()
        : base("missing postgres settings") { }
}
