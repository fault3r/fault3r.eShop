
using System;
using FluentValidation;

namespace UserService.Application.UseCases.Commands.LoginUserUseCase;

public sealed class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(p => p.Identity)
            .EmailAddress()
            .NotEmpty();

        RuleFor(p => p.Password)
            .Length(8,100)
            .NotEmpty();
    }
}
