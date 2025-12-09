
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingSerilogSettingsException : InfrastructureException
{
    public MissingSerilogSettingsException()
        : base("missing serilog logger settings") { }
}
