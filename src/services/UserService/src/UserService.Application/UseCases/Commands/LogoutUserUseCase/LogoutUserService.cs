using System;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.Commands.LogoutUserUseCase;

public class LogoutUserService : ILogoutUserService
{
    public async Task<Result> ExecuteAsync(
        string sessionId,
        string refreshToken,
        CancellationToken cancellationToken = default)
}
