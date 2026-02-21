
using System;
using UserService.Application.UseCases.Queries.UserProfileUseCase;
using UserService.Domain.Contracts;

namespace UserService.Application.Interfaces;

public interface IUserProfileService
{
    Task<Result<UserProfileResult>> ExecuteAsync(
        string sessionId,
        CancellationToken cancellationToken);
}
