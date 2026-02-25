
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Commands.RegisterUserUseCase;

public sealed class RegisterUserCommandHandler(
    IRegisterUserService registerUserService,
    IValidator<RegisterUserCommand> validator,
    ILogger<RegisterUserCommandHandler> logger
) : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult>>
{
    private readonly IRegisterUserService _registerService = registerUserService;
    private readonly IValidator<RegisterUserCommand> _validator = validator;
    private readonly ILogger<RegisterUserCommandHandler> _logger = logger;

    public async Task<Result<RegisterUserResult>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            _logger.LogWarning("Validation failed for {Email}: {Error}", request.Email.Trim(), errors);

            return Result<RegisterUserResult>.Failure(errors);
        }

        var result = await _registerService.ExecuteAsync(
            email: request.Email,
            password: request.Password,
            fullName: request.FullName,
            cancellationToken: cancellationToken
        );

        return result;
    }
}
