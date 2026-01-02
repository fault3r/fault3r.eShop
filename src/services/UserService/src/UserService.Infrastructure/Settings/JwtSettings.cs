
using System;

namespace UserService.Infrastructure.Settings;

public sealed class JwtSettings
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SigningKey { get; init; }
    public required int TokenLifetimeMinutes { get; init; }
}