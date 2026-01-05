
using System;

namespace UserService.Application.UseCases.Commands.RegisterUserUseCase;

public sealed record RegisterUserResult(
    string UserId,
    string Email,
    string FullName
);