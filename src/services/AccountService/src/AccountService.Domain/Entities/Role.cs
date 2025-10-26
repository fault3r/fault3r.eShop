
using System;
using AccountService.Domain.Entities.Base;

namespace AccountService.Domain.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }

        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
