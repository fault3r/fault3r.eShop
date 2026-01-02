
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;
using UserService.Domain.Repositories;
using UserService.Domain.Security;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.DomainServices;

public class UserDomainService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : IUserDomainService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<bool> VerifyCanCreateAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var exists = await _userRepository
            .GetByEmailAsync(email, cancellationToken);

        return exists is null;
    }

    public async Task<User?> VerifyCredentialAsync(
       string identity,
       string password,
       CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(Identity.From(identity), cancellationToken);

        if (user is null) return null;

        if (!_passwordHasher.Verify(password, user.PasswordHash))
            return null;

        return user;
    }
}