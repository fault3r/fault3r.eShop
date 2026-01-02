
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface IRegisterUserService
{
    Task<Result<User>> ExecuteAsync(
        string email,
        string password,
        string fullName,
        CancellationToken ct = default
    );
}
