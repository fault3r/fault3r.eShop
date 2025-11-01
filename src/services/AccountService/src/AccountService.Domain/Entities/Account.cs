
using System;
using AccountService.Domain.Entities.Base;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Entities
{
    public class Account : BaseEntity
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Name { get; set; }

        public required Role Role { get; set; }

        public bool IsConfirmed { get; set; } = false;
    }
}