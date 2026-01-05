
using System;

namespace UserService.Application.UseCases.Commands.RefreshAuthUseCase;

public sealed record RefreshAuthResult(
    string AccessToken,
    string RefreshToken
);
