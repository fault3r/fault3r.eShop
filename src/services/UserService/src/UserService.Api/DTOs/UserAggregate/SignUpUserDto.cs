
using System;

namespace UserService.Api.DTOs.UserAggregate;
public sealed record SignUpUserDto(
    string Email,
    string Password,
    string FullName
);
