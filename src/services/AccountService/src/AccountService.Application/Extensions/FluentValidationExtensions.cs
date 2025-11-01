
using System;
using FluentValidation.Results;

namespace AccountService.Application.Extensions
{
    public static class FluentValidationExtensions
    {
        public static string ToErrorString(this ValidationResult result)
        {
            return string.Join(
                separator: "; ",
                values: result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
        }
    }
}