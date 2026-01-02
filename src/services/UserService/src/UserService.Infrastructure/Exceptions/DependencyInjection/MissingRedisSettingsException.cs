
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingRedisSettingsException : InfrastructureException
{
    public MissingRedisSettingsException()
        : base("missing remote directory server (redis) settings") { }
}
