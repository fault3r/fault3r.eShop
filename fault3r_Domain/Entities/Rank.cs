

using System;
using System.Collections.Generic;

namespace fault3r_Domain.Entities
{
    public class Rank: BaseEntity
    {
        public int RankNumber { get; set; }

        public string RankName { get; set; }

        public Guid? ForumId { get; set; }
        public virtual Forum Forum { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }

}
