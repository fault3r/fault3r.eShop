
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingSessionSettingsException : InfrastructureException
{
    public MissingSessionSettingsException()
        : base("missing application session settings") { }
}
