
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingAppSettingsException : InfrastructureException
{
    public MissingAppSettingsException()
        : base("missing app settings") { }
}
