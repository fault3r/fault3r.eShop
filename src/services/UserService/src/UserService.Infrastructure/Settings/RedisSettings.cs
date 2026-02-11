
using System;

namespace UserService.Infrastructure.Settings;

public sealed class RedisSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }

    public string ConnectionString
        => $"{Host}:{Port}";
}
