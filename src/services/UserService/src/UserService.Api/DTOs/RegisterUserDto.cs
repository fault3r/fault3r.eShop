
using System;

namespace UserService.Api.DTOs;

public sealed record RegisterUserDto(
    string Email,
    string Password,
    string FullName
);
