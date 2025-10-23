
using System;

namespace AccountService.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = [];
    }
}
