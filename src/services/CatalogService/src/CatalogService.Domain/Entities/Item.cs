
using System;

namespace CatalogService.Domain.Entities
{
    public class Item
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required decimal Price { get; set; }

        public required ICollection<string> Pictures { get; set; }

        public required DateTime Updated { get; set; }
    }
}