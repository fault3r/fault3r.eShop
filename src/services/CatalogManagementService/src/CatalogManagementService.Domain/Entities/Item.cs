
using System;

namespace CatalogManagementService.Domain.Entities
{
    public class Item
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public decimal Price { get; set; }

        public required ICollection<string> Pictures { get; set; }

        public DateTime Updated { get; set; }
    }
}