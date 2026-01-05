
using System;

namespace UserService.Application.UseCases.Commands.LoginUserUseCase;

public sealed record LoginUserResult(
    string AccessToken,
    string RefreshToken
);
