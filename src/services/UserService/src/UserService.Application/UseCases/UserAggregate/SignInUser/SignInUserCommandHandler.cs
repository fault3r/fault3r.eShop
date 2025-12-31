
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.UserAggregate.SignInUser;

public class SignInUserCommandHandler(
    ISignInUserService signInUserService,
    IValidator<SignInUserCommand> validator,
    ILogger<SignInUserCommandHandler> logger)
        : IRequestHandler<SignInUserCommand, Result<SignInUserResult>>
{
    private readonly ISignInUserService _signInService = signInUserService;
    private readonly IValidator<SignInUserCommand> _validator = validator;
    private readonly ILogger<SignInUserCommandHandler> _logger = logger;

    public async Task<Result<SignInUserResult>> Handle(
        SignInUserCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            return Result<SignInUserResult>.Failure($"Validation failed: {errors}!");
        }

        var result = await _signInService.ExecuteAsync(
            request.Identity,
            request.Password,
            cancellationToken
        );

        return result;
    }

}
