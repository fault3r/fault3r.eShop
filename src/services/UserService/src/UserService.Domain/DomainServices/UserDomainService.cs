
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.DomainServices;

public class UserDomainService(
    IUserRepository userRepository) : IUserDomainService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<bool> CanCreateAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var exists = await _userRepository
            .GetByEmailAsync(email, cancellationToken);

        return exists is null;
    }

    public async Task<Result<User>> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(Identity.From(id));

        if (user is null)
            return Result<User>.Failure("user not found");

        return Result<User>.Success(user);
    }
}