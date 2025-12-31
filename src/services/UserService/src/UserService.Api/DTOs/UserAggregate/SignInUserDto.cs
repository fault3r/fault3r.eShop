
using System;

namespace UserService.Api.DTOs.UserAggregate;

public sealed record SignInUserDto(
    string Identity,
    string Password
);
