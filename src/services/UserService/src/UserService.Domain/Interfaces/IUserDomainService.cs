
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Interfaces;

public interface IUserDomainService
{
    Task<bool> VerifyCanCreateAsync(
        Email email,
        CancellationToken cancellationToken
    );

    Task<Result<User>> VerifyCredentialsAsync(
        string identity,
        string password,
        CancellationToken cancellationToken
    );
}
