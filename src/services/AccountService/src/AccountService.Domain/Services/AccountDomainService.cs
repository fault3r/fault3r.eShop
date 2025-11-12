
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.Common;
using AccountService.Domain.Exceptions;
using AccountService.Domain.Exceptions.Persistence;
using AccountService.Domain.Factories;
using AccountService.Domain.Repositories;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Services;

public class AccountDomainService
{
    private readonly IAccountRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IOutboxMessageService _outbox;

    public AccountDomainService(
        IAccountRepository repository, IUnitOfWork uow, IOutboxMessageService outbox)
    {
        _repository = repository
            ?? throw new MissingRepositoryException();
        _uow = uow
            ?? throw new MissingUnitOfWorkException();
        _outbox = outbox
            ?? throw new MissingOutBoxException();
    }

    public async Task<Result<Account>> SignUpAsync(
        string fullName, string emailAddress, string passwordHash, CancellationToken cancellationToken = default)
    {
        var email = Email.From(emailAddress);
        var existing = await _repository.GetByEmailAsync(email, cancellationToken);
        if (existing is not null)
            return Result<Account>.Failure("email address already in use");

        var createResult = AccountFactory.CreateNew(fullName, emailAddress, passwordHash);
        if (createResult.IsFailure)
            return createResult;

        var account = createResult.Value!;
        await _repository.AddAsync(account, cancellationToken);
        int result = await _uow.CommitAsync(cancellationToken);

        if (result <= 0)
            return Result<Account>.Failure("failed to persist account");

        return Result<Account>.Success(account);
    }
}
