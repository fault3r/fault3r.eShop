
using System;

namespace UserService.Api.DTOs;

public sealed record SignUpUserDto(
    string Email,
    string Password,
    string FullName
);
