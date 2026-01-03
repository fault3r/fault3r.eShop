
using System;

namespace UserService.Application.UseCases.RefreshAuthUseCase;

public sealed record RefreshAuthResult(
    string AccessToken,
    string RefreshToken
);
