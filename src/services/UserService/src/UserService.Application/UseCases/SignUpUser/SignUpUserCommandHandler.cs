
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.SignUpUser;

public class SignUpUserCommandHandler
    : IRequestHandler<SignUpUserCommand, Result<User>>
{
    private readonly ISignUpUserService _signUpService;
    private readonly IValidator<SignUpUserCommand> _validator;
    private readonly ILogger<SignUpUserCommandHandler> _logger;

    public SignUpUserCommandHandler(
        ISignUpUserService signUpUserService,
        IValidator<SignUpUserCommand> validator,
        ILogger<SignUpUserCommandHandler> logger)
    {
        _signUpService = signUpUserService
            ?? throw new ArgumentNullException(nameof(signUpUserService));

        _validator = validator
            ?? throw new ArgumentNullException(nameof(validator));

        _logger = logger;
    }

    public async Task<Result<User>> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling request..");

        var validation = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            _logger.LogWarning("Validation failed with the following errors: {Errors}!", errors);

            return Result<User>.Failure($"Validation failed: {errors}!");
        }

        var result = await _signUpService
            .ExecuteAsync(
                request.Email,
                request.Password,
                request.FullName,
                request.CorrelationId,
                cancellationToken
            );

        _logger.LogInformation("Request handled successfully.");

        return result;
    }
}
