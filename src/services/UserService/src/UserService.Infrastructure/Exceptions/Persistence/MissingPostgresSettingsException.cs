
using System;

namespace UserService.Infrastructure.Exceptions.Persistence;

public class MissingPostgresSettingsException : InfrastructureException
{
    public MissingPostgresSettingsException()
        : base("missing postgres settings") { }
}
