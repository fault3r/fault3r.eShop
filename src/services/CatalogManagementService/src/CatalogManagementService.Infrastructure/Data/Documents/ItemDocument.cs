
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

        public DateTime Updated { get; set; }

        public ItemDocument()
        {
            Description = string.Empty;
            Updated = DateTime.UtcNow;
        }

        public Item ToDomain() => new()
        {
            Id = Id.ToString(),
            Name = Name,
            Description = Description,
            Price = Price,
            Updated = Updated,
        };
        
    }
}