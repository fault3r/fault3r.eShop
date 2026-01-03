
using System;

namespace UserService.Application.UseCases.RegisterUserUseCase;

public sealed record RegisterUserResult(
    string UserId,
    string Email,
    string FullName
);