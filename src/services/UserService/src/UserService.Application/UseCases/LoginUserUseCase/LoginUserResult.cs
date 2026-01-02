
using System;

namespace UserService.Application.UseCases.LoginUserUseCase;

public sealed record LoginUserResult(
    string AccessToken,
    string RefreshToken
);
