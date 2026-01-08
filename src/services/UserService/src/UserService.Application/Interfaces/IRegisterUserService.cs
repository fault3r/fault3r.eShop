
using System;
using UserService.Application.UseCases.Commands.RegisterUserUseCase;
using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface IRegisterUserService
{
    Task<Result<RegisterUserResult>> ExecuteAsync(
        string email,
        string password,
        string fullName,
        CancellationToken cancellationToken = default
    );
}
