

using System.Collections.Generic;

namespace fault3r_Domain.Entities
{
    public class Role: BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }

}
