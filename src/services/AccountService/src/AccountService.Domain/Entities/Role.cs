
using System;

namespace AccountService.Domain.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }

        public virtual required ICollection<Account> Accounts { get; set; }
    }
}
