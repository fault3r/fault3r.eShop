
using System;

namespace AccountService.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public string Name { get; set; }

        public Guid RoleId { get; set; }
        public virtual required Role Role { get; set; }

        public Account()
        {
            Id = Guid.NewGuid();
            Name = "";
        }
    }
}