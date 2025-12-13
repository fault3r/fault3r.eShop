
using System;
using FluentValidation;
using UserService.Application.Interfaces;
using UserService.Application.Security;
using UserService.Domain.Aggregates;
using UserService.Domain.Common;
using UserService.Domain.Exceptions;
using UserService.Domain.Factories;
using UserService.Domain.Interfaces;
using UserService.Domain.UnitOfWork;
using UserService.Domain.ValueObjects;

namespace UserService.Application.UseCases.SignUpUser;

public sealed class SignUpUserService : ISignUpUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserDomainService _domainService;
    private readonly IValidator<SignUpUserCommand> _validator;
    private readonly IPasswordHasher _passwordHasher;

    public SignUpUserService(
        IUnitOfWork uow,
        IUserDomainService domainService,
        IValidator<SignUpUserCommand> validator,
        IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _domainService = domainService;
        _validator = validator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<User>> ExecuteAsync(
        SignUpUserCommand command,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        var validation = await _validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
            return Result<User>.Failure(
                string.Join("\n", validation.Errors.Select(e => e.ErrorMessage))
            );        

        User user;
        Email email;
        PasswordHash passwordHash;
        FullName fullName;
        try
        {
            email = Email.Parse(command.Email);

            string hashed = _passwordHasher.Hash(command.Password);
            passwordHash = PasswordHash.Parse(hashed);

            fullName = FullName.Parse(command.FullName);

            user = UserFactory.CreateNew(email, passwordHash, fullName);
        }
        catch (DomainException ex)
        {
            return Result<User>.Failure($"Error signing up user: {ex.Message}");
        }
        
        var canCreate = await _domainService.CanCreateUserAsync(email, cancellationToken);
        if (!canCreate)
            return Result<User>.Failure("User with this email already exists!");

        try
        {
            await _uow.Users.CreateAsync(user, cancellationToken);
            await _uow.Outbox.EnqueueAsync(user.DomainEvents, correlationId, cancellationToken);
            await _uow.CommitAsync(cancellationToken);
            user.ClearEvents();
        }
        catch
        {
            return Result<User>.Failure($"An unexpected error occurred!");
        }

        return Result<User>.Success(user);
    }
}
