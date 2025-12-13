
using System;
using UserService.Application.UseCases.SignUpUser;
using UserService.Domain.Aggregates;
using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface ISignUpUserService
{
    Task<Result<User>> ExecuteAsync(
        string email,
        string password,
        string fullName,
        string correlationId,
        CancellationToken cancellationToken = default
    );
}
