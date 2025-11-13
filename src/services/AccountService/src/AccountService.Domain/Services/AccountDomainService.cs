
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.Common;
using AccountService.Domain.Exceptions;
using AccountService.Domain.Factories;
using AccountService.Domain.Repositories;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Services;

public class AccountDomainService
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IOutbox _outbox;

    public AccountDomainService(
        IRepository repository, IUnitOfWork uow, IOutbox outbox)
    {
        _repository = repository
            ?? throw new DomainException("Repository is required");
        _uow = uow
            ?? throw new DomainException("UnitOfWork is required");
        _outbox = outbox
            ?? throw new DomainException("Outbox is required");
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
        await _repository.CreateAsync(account, cancellationToken);
        bool result = await _uow.CommitAsync(cancellationToken);

        if (!result)
            return Result<Account>.Failure("failed to persist account");

        return Result<Account>.Success(account);
    }
}
