
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.RegisterUserUseCase;

public class RegisterUserCommandHandler(
    IRegisterUserService registerUserService,
    IValidator<RegisterUserCommand> validator,
    ILogger<RegisterUserCommandHandler> logger)
        : IRequestHandler<RegisterUserCommand, Result<User>>
{
    private readonly IRegisterUserService _registerService = registerUserService;
    private readonly IValidator<RegisterUserCommand> _validator = validator;
    private readonly ILogger<RegisterUserCommandHandler> _logger = logger;

    public async Task<Result<User>> Handle(
        RegisterUserCommand request,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var validation = await _validator.ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            return Result<User>.Failure($"Validation failed: {errors}");
        }

        var result = await _registerService.ExecuteAsync(
            request.Email,
            request.Password,
            request.FullName,
            ct
        );

        return result;
    }
}
