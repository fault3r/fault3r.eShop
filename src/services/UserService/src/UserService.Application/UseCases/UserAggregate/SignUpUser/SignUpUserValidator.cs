
using System;
using FluentValidation;

namespace UserService.Application.UseCases.SignUpUser;

public sealed class SignUpUserValidator : AbstractValidator<SignUpUserCommand>
{
    public SignUpUserValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotEmpty()
            .Length(8, 50);

        RuleFor(p => p.FullName)
            .NotEmpty()
            .Length(2, 50);
    }
}
