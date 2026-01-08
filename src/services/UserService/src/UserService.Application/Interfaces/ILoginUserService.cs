
using System;
using UserService.Application.UseCases.Commands.LoginUserUseCase;
using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface ILoginUserService
{
    Task<Result<LoginUserResult>> ExecuteAsync(
        string identity,
        string password,
        CancellationToken cancellationToken = default
    );
}
