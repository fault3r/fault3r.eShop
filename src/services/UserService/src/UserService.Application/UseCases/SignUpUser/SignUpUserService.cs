
using System;
using UserService.Application.Interfaces;
using UserService.Application.Security;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Exceptions;
using UserService.Domain.Factories;
using UserService.Domain.UnitOfWork;
using UserService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using UserService.Domain.Services.UserService;

namespace UserService.Application.UseCases.SignUpUser;

public sealed class SignUpUserService : ISignUpUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserDomainService _domainService;
    private readonly IPasswordHasher _hasher;
    private readonly ILogger<SignUpUserService> _logger;

    public SignUpUserService(
        IUnitOfWork unitOfWork,
        IUserDomainService userDomainService,
        IPasswordHasher passwordHasher,
        ILogger<SignUpUserService> logger)
    {
        _uow = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));

        _domainService = userDomainService
            ?? throw new ArgumentNullException(nameof(userDomainService));

        _hasher = passwordHasher
            ?? throw new ArgumentNullException(nameof(passwordHasher));

        _logger = logger;
    }

    public async Task<Result<User>> ExecuteAsync(
        string email,
        string password,
        string fullName,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing service for Email: {Email}..", email.Trim());

        User user;
        Email vEmail;
        PasswordHash vPasswordHash;
        FullName vFullName;
        try
        {
            vEmail = Email.Parse(email);

            string hashed = _hasher.Hash(password);
            vPasswordHash = PasswordHash.Parse(hashed);

            vFullName = FullName.Parse(fullName);

            user = UserFactory.CreateNew(vEmail, vPasswordHash, vFullName);

            _logger.LogInformation("Instance created successfully..");
        }
        catch (DomainException exception)
        {
            _logger.LogWarning("Domain validation failed with the following exception: {Exception}!", exception.Message);

            return Result<User>.Failure($"Sign up failed: {exception.Message}!");
        }

        _logger.LogInformation("Checking email exists..");

        var canCreate = await _domainService.CanCreateUserAsync(vEmail, cancellationToken);
        if (!canCreate)
        {
            _logger.LogWarning("User with this email already exists!");

            return Result<User>.Failure("Sign up failed: User with this email already exists!");
        }

        _logger.LogInformation("Persisting instance to database..");

        await _uow.UserRepository.CreateAsync(user, cancellationToken);
        await _uow.Outbox.EnqueueAsync(user.DomainEvents, correlationId, cancellationToken);
        await _uow.CommitAsync(cancellationToken);
        user.ClearEvents();

        _logger.LogInformation("User successfully created with Id: {Id}.", user.Id.ToString());

        return Result<User>.Success(user);
    }
}
