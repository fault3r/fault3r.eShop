
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

        public Role Role { get; set; } = Role.User();

        public bool IsConfirmed { get; set; } = false;
    }
}