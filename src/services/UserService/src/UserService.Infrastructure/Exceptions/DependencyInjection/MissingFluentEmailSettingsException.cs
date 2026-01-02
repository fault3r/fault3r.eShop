
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingFluentEmailSettingsException : InfrastructureException
{
    public MissingFluentEmailSettingsException()
        : base("missing fluent email settings") { }
}
