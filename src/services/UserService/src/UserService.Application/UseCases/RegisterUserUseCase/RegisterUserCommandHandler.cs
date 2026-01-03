
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.RegisterUserUseCase;

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
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var validation = await _validator.ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            _logger.LogWarning("Validation failed: {Error}", errors);

            return Result<RegisterUserResult>.Failure($"Validation failed: {errors}");
        }

        var result = await _registerService.ExecuteAsync(
            email: request.Email,
            password: request.Password,
            fullName: request.FullName,
            ct: ct
        );

        return result;
    }
}
