
using System;
using UserService.Application.UseCases.UserAggregate.SignInUser;
using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface ISignInUserService
{
    Task<Result<SignInUserResult>> ExecuteAsync(
        string identity,
        string password,
        CancellationToken cancellationToken = default
    );
}
