

using System;

namespace fault3r_Domain.Entities
{
    public class Topic: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }

        public Guid ForumId { get; set; }
        public virtual Forum Forum { get; set; }
    }
}
