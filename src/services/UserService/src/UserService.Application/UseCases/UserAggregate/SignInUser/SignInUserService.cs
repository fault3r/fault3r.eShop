
using System;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.UserAggregate.SignInUser;

public sealed class SignInUserService : ISignInUserService
{
    public async Task<Result<SignInUserResult>> ExecuteAsync(
        string identity,
        string password,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        
    }
}
