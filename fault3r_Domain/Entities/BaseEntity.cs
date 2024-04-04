

using System;

namespace fault3r_Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();       
    }
}
