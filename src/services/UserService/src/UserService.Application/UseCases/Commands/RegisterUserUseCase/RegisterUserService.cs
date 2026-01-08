
using System;
using UserService.Application.Interfaces;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Exceptions;
using UserService.Domain.Factories;
using UserService.Domain.UnitOfWork;
using UserService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using UserService.Domain.Interfaces;
using UserService.Domain.Security;

namespace UserService.Application.UseCases.Commands.RegisterUserUseCase;

public sealed class RegisterUserService(
    IUnitOfWork unitOfWork,
    IUserDomainService userDomainService,
    ICorrelationContext correlation,
    IPasswordHasher passwordHasher,
    ILogger<RegisterUserService> logger
) : IRegisterUserService
{
    private readonly IUnitOfWork _uow = unitOfWork;
    private readonly IUserDomainService _userService = userDomainService;
    private readonly ICorrelationContext _correlation = correlation;
    private readonly IPasswordHasher _hasher = passwordHasher;
    private readonly ILogger<RegisterUserService> _logger = logger;

    public async Task<Result<RegisterUserResult>> ExecuteAsync(
        string email,
        string password,
        string fullName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

        _logger.LogInformation("Registering user with '{Email}' email address…", email.Trim());

        User user;
        Email voEmail;
        PasswordHash voPasswordHash;
        FullName voFullName;
        try
        {
            voEmail = Email.From(email);

            string hashed = _hasher.Hash(password);
            voPasswordHash = PasswordHash.From(hashed);

            voFullName = FullName.From(fullName);

            user = UserFactory.Create(voEmail, voPasswordHash, voFullName);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain validation failed: {Error}", ex.Message);

            return Result<RegisterUserResult>.Failure(ex.Message);
        }

        var canCreate = await _userService.VerifyCanCreateAsync(voEmail, cancellationToken);
        if (!canCreate)
        {
            _logger.LogWarning("User with this email address already exists!");

            return Result<RegisterUserResult>.Failure("User with this email address already exists!");
        }

        await _uow.UserRepository.CreateAsync(user, cancellationToken);
        await _uow.Outbox.EnqueueAsync(user.Events, _correlation.CorrelationId, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        await _uow.Notification.DispatchAsync(user.Events, cancellationToken);

        user.ClearEvents();

        _logger.LogInformation("User successfully registered with '{Id}' identity.", user.Id.ToString());

        return Result<RegisterUserResult>.Success(
            new RegisterUserResult(user.Id, user.Email, user.FullName));
    }
}
