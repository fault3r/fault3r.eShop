
using System;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Services.UserService;

public interface IUserDomainService
{
    Task<bool> CanCreateUserAsync(
        Email email,
        CancellationToken cancellationToken = default
    );
}
