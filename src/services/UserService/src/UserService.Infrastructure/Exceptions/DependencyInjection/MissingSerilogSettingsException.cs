
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingSerilogSettingsException : InfrastructureException
{
    public MissingSerilogSettingsException()
        : base("missing serilog logger settings") { }
}
