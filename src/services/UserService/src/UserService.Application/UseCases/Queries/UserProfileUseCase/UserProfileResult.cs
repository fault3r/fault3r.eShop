
using System;

namespace UserService.Application.UseCases.Queries.UserProfileUseCase;

public sealed record UserProfileResult(
    string UserId,
    string Email,
    string FullName,
    string Role,
    string Status
);