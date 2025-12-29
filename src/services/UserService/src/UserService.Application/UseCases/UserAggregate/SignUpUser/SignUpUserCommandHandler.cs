
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.UserAggregate.SignUpUser;

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
        _signUpService = signUpUserService;
        _validator = validator;
        
        _logger = logger;
    }

    public async Task<Result<User>> Handle(
        SignUpUserCommand request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        _logger.LogInformation("Handling request for {Email} email..", request.Email.Trim());

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            _logger.LogWarning("Validation failed: {Errors}!", errors);

            return Result<User>.Failure($"Validation failed: {errors}!");
        }

        var result = await _signUpService.ExecuteAsync(
            request.Email,
            request.Password,
            request.FullName,
            cancellationToken
        );

        _logger.LogInformation("Request handled successfully.");

        return result;
    }
}
