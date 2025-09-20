
using System;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Infrastructure.Data.Documents.BaseDocument;

namespace CatalogManagementService.Infrastructure.Data.Documents
{
    public class ItemDocument : ItemBaseDocument
    {
        public required string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public ICollection<string> Pictures { get; set; } = [];

        public DateTime Updated { get; set; }

        public ItemDocument()
        {
            Description = "";
            Pictures = [];
            Updated = DateTime.UtcNow;
        }

        public Item ToDomain()
        {
            return new Item
            {
                Id = this.Id.ToString(),
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                Pictures = this.Pictures,
                Updated = this.Updated,
            };
        }
    }
}