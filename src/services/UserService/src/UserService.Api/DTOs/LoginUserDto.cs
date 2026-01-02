
using System;

namespace UserService.Api.DTOs;

public sealed record LoginUserDto(
    string Identity,
    string Password
);
