
using System;
using UserService.Application.UseCases.Commands.RefreshAuthUseCase;
using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface IRefreshAuthService
{
    Task<Result<RefreshAuthResult>> ExecuteAsync(
        string accessToken,
        string refreshToken,
        CancellationToken ct = default
    );
}
