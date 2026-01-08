
using System;
using FluentValidation;

namespace UserService.Application.UseCases.Commands.LogoutUserUseCase;

public sealed class LogoutUserValidator : AbstractValidator<LogoutUserCommand>
{
    public LogoutUserValidator()
    {
        RuleFor(p => p.SessionId)
            .NotEmpty();
    }
}
