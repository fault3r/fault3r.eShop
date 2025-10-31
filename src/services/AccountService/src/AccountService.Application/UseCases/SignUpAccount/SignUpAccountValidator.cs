
using System;
using AccountService.Application.DTOs;
using FluentValidation;

namespace AccountService.Application.UseCases.SignUpAccount
{
    public class SignUpAccountValidator : AbstractValidator<SignUpAccountDto>
    {
        public SignUpAccountValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty()
                .Length(6, 50);

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty()
                .Length(6, 50);

            RuleFor(p => p)
                .Must(p => p.Password == p.ConfirmPassword)
                    .WithMessage("Password and ConfirmPassword do not match!");

            RuleFor(p => p.Name)
                .NotEmpty()
                .Length(2, 50);
        }
    }
} 