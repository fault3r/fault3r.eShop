
using System;

namespace AccountService.Domain.Entities
{
    public class Account : BaseEntity
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Name { get; set; }

        public Guid RoleId { get; set; }
        public virtual required Role Role { get; set; }
    }
}