
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Interfaces;

public interface IUserDomainService
{
    Task<bool> VerifyCanCreateAsync(
        Email email,
        CancellationToken ct = default
    );

    Task<User?> VerifyCredentialAsync(
        string identity,
        string password,
        CancellationToken ct = default
    );
}
