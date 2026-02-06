
using System;

namespace UserService.Infrastructure.Settings;

public sealed class RabbitmqSettings
{
    public required string HostName { get; init; }
    public int Port { get; init; } = 5672;
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string Exchange { get; init; }
    public required string ExchangeType { get; init; }
}