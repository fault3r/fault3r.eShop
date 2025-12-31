
using System;

namespace UserService.Infrastructure.Settings;

public class RedisSetting
{
    public required string Host { get; set; }
    public required int Port { get; set; }
}
