
using System;

namespace AccountService.Application.DTOs
{
    public class SignUpAccountDto
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string ConfirmPassword { get; set; }

        public required string Name { get; set; }
    }
}
