
using System;

namespace AccountService.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public Role(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Accounts = [];
        }
    }
}
