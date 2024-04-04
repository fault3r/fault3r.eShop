
using System;
using System.Collections.Generic;

namespace fault3r_Domain.Entities
{
    public class Forum : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public Guid? ParentForumId { get; set; }
        public virtual Forum ParentForum { get; set; }

        public virtual ICollection<Forum> SubForums { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }

        public virtual ICollection<Rank> Ranks { get; set; }
    }
}
