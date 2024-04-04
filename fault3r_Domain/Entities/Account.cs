

using System;
using System.Collections.Generic;

namespace fault3r_Domain.Entities
{
    public class Account: BaseEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public Guid RankId { get; set; }
        public virtual Rank Rank { get; set; }

        public string Date { get; set; }

        public string Bio { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }

    }

}
