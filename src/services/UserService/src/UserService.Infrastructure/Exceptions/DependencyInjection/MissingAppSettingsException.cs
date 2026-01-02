
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingAppSettingsException : InfrastructureException
{
    public MissingAppSettingsException()
        : base("missing application settings") { }
}
