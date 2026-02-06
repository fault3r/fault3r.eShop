
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingRabbitmqSettingsException : InfrastructureException
{
    public MissingRabbitmqSettingsException()
        : base("missing rabbitmq settings") { }
}
