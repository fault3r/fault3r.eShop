
using System;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Interfaces;

public interface IUserDomainService
{
    Task<bool> CanCreateAsync(
        Email email,
        CancellationToken cancellationToken = default
    );
}
