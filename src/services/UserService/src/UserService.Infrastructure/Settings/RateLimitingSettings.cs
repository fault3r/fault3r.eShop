
using System;

namespace UserService.Infrastructure.Settings;

public sealed class RateLimitingSettings
{
    public required int PermitLimit { get; init; }
    public required int Window { get; init; }
    public required int QueueLimit { get; init; }
    public required bool IsOldestFirst { get; init; }

    public required RefAuthRateLimitSettings RefAuthRateLimit { get; init; }

    public sealed class RefAuthRateLimitSettings
    {
        public required int PermitLimit { get; init; }
        public required int Window { get; init; }
    }
}
