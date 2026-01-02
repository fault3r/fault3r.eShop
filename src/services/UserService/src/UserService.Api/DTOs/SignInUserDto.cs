
using System;

namespace UserService.Api.DTOs;

public sealed record SignInUserDto(
    string Identity,
    string Password
);
