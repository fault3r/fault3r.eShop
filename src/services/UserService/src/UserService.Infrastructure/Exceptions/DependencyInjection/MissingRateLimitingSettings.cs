
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingRateLimitingSettings : InfrastructureException
{
    public MissingRateLimitingSettings()
        : base("missing rate limiting settings") { }
}
