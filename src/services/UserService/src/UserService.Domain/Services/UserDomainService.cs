
using System;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Services;

public class UserDomainService : IUserDomainService
{
    private readonly IUserRepository _userRepository;

    public UserDomainService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> CanCreateUserAsync(
        Email email, CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.GetByEmailAsync(email, cancellationToken);
        return exists is null;
    }
}