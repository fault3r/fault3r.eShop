
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection.Serilog;

public class MissingLoggerSettingsException : InfrastructureException
{
    public MissingLoggerSettingsException()
        : base("missing serilog logger settings") { }
}
