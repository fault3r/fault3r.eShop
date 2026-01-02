
using System;
using FluentValidation;

namespace UserService.Application.UseCases.UserAggregate.SignUpUser;

public sealed class SignUpUserValidator : AbstractValidator<SignUpUserCommand>
{
    public SignUpUserValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotEmpty()
            .Length(6, 100);

        RuleFor(p => p.FullName)
            .NotEmpty()
            .Length(2, 100);
    }
}
