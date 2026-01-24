
using System;

namespace UserService.Domain.Security.Authentication;

public sealed class SessionData
{
    public required string SessionId { get; init; }
    public required string DeviceId { get; init; }
    public required string IpAddress { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    public required string RefreshTokenHash { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
    public DateTimeOffset LastAccessedAt { get; set; }

    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required string FullName { get; init; }
    public required string Role { get; init; }
    public required string Status { get; init; }
}
