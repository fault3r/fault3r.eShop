
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingSerilogSettingException : InfrastructureException
{
    public MissingSerilogSettingException()
        : base("missing serilog logger setting") { }
}
