
using System;

namespace UserService.Api.DTOs.User;

public sealed record SignUpUserDto(
    string Email,
    string Password,
    string FullName
);
