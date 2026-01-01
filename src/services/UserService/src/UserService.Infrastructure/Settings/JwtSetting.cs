
using System;

namespace UserService.Infrastructure.Settings;

public class JwtSetting
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string SigningKey { get; set; }
    public required int AccessTokenLifetimeMinutes { get; set; }
}