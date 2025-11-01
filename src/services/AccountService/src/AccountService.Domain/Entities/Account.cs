
using System;
using AccountService.Domain.Entities.Base;

namespace AccountService.Domain.Entities
{
    public class Account : BaseEntity
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Name { get; set; }


        public bool IsConfirmed { get; set; }
    }
}