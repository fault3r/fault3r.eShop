
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.LoginUserUseCase;

public sealed class LoginUserCommandHandler(
    ILoginUserService loginUserService,
    IValidator<LoginUserCommand> validator,
    ILogger<LoginUserCommandHandler> logger)
        : IRequestHandler<LoginUserCommand, Result<LoginUserResult>>
{
    private readonly ILoginUserService _loginService = loginUserService;
    private readonly IValidator<LoginUserCommand> _validator = validator;
    private readonly ILogger<LoginUserCommandHandler> _logger = logger;

    public async Task<Result<LoginUserResult>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            return Result<LoginUserResult>.Failure($"Validation failed: {errors}!");
        }

        var result = await _loginService.ExecuteAsync(
            request.Identity,
            request.Password,
            cancellationToken
        );

        return result;
    }

}
