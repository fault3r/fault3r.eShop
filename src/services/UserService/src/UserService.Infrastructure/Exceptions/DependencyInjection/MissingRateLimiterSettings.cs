
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingRateLimiterSettings : InfrastructureException
{
    public MissingRateLimiterSettings()
        : base("missing rate limiter settings") { }
}
