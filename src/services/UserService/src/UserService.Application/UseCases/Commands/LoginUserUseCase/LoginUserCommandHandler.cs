
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;
using System.Linq;

namespace UserService.Application.UseCases.Commands.LoginUserUseCase;

public sealed class LoginUserCommandHandler(
    ILoginUserService loginUserService,
    IValidator<LoginUserCommand> validator,
    ILogger<LoginUserCommandHandler> logger
) : IRequestHandler<LoginUserCommand, Result<LoginUserResult>>
{
    private readonly ILoginUserService _loginService = loginUserService;
    private readonly IValidator<LoginUserCommand> _validator = validator;
    private readonly ILogger<LoginUserCommandHandler> _logger = logger;

    public async Task<Result<LoginUserResult>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            _logger.LogWarning("Validation failed: {Error}", errors);

            return Result<LoginUserResult>.Failure(errors);
        }

        var result = await _loginService.ExecuteAsync(
            identity: request.Identity,
            password: request.Password,
            cancellationToken: cancellationToken
        );

        return result;
    }

}
