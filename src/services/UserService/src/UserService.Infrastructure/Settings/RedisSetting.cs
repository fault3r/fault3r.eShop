
using System;

namespace UserService.Infrastructure.Settings;

public class RedisSetting
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    
    public required string SessionKey { get; set; }
    public required string UserSessionsKey { get; set; }

    public required int SessionLifetimeDays { get; set; }

    public string ToConnectionString()
        => $"{Host}:{Port}";
}
