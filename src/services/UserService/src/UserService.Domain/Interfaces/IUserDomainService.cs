
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Interfaces;

public interface IUserDomainService
{
    Task<bool> CanCreateAsync(
        Email email,
        CancellationToken cancellationToken = default
    );

    Task<Result<User>> GetUserByIdAsync(string id);
}
