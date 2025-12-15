
using System;
using UserService.Domain.Aggregates.UserAggregate;
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
