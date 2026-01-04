
using System;

namespace UserService.Infrastructure.Settings;

public sealed class RedisSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    
    public required string SessionKey { get; init; }
    public required string UserSessionsKey { get; init; }

    public required int MaxSessionsPerUser { get; init; }

    public string ToConnectionString()
        => $"{Host}:{Port}";
}
