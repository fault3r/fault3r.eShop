
using System;
using UserService.Application.UseCases.RefreshAuthUseCase;
using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface IRefreshAuthService
{
    Task<Result<RefreshAuthResult>> ExecuteAsync(
        string expiredAccessToken,
        string providedRefreshToken,
        CancellationToken cancellationToken = default
    );
}
