
using System;

namespace UserService.Application.UseCases.UserAggregate.SignInUser;

public sealed class SignInUserResult
{
    public required string AccessToken { get; init; }
    public DateTime AccessTokenExpiration { get; init; }

    public required string RefreshToken { get; init; }
    public DateTime RefreshTokenExpiration { get; init; }

    public required string SessionId { get; init; }

    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required string FullName { get; init; }
    public required string Role { get; init; }
    public required string Status { get; init; }
}
