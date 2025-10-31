
using System;

namespace AccountService.Application.UseCases.SignUpAccount
{
    public class SignUpAccountResult
    {
        public bool Success { get; set; } = false;

        public string? Message { get; set; }
    }
}
