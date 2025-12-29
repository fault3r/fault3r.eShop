
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
using UserService.Application.Security;

namespace UserService.Application.UseCases.UserAggregate.SignUpUser;

public sealed class SignUpUserService : ISignUpUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserDomainService _userService;
    private readonly ICorrelationContext _correlation;
    private readonly IPasswordHasher _hasher;
    private readonly ILogger<SignUpUserService> _logger;

    public SignUpUserService(
        IUnitOfWork unitOfWork,
        IUserDomainService userDomainService,
        ICorrelationContext correlation,
        IPasswordHasher passwordHasher,
        ILogger<SignUpUserService> logger)
    {
        _uow = unitOfWork;
        _userService = userDomainService;
        _correlation = correlation;
        _hasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<User>> ExecuteAsync(
        string email,
        string password,
        string fullName,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing service..");

        User user;
        Email vEmail;
        PasswordHash vPasswordHash;
        FullName vFullName;
        try
        {
            vEmail = Email.From(email);

            string hashed = _hasher.Hash(password);
            vPasswordHash = PasswordHash.From(hashed);

            vFullName = FullName.From(fullName);

            user = UserFactory.Create(vEmail, vPasswordHash, vFullName);

            _logger.LogInformation("Instance created successfully..");
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain validation failed with the following exception: {Exception}!", ex.Message);

            return Result<User>.Failure($"Sign up failed: {ex.Message}!");
        }

        _logger.LogInformation("Checking email exists..");

        var canCreate = await _userService.CanCreateAsync(vEmail, cancellationToken);
        if (!canCreate)
        {
            _logger.LogWarning("User with this email already exists!");

            return Result<User>.Failure("Sign up failed: User with this email already exists!");
        }

        _logger.LogInformation("Persisting instance to database..");

        await _uow.UserRepository.CreateAsync(user, cancellationToken);
        await _uow.Outbox.EnqueueAsync(user.Events, correlationId, cancellationToken);
        await _uow.CommitAsync(cancellationToken);
        user.ClearEvents();

        _logger.LogInformation("User successfully created with Id: {Id}.", user.Id.ToString());

        return Result<User>.Success(user);
    }
}
