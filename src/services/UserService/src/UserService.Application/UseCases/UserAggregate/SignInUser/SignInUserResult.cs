
using System;

namespace UserService.Application.UseCases.UserAggregate.SignInUser;

public sealed class SignInUserResult
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}
