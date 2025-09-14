using System;
using CatalogService.Infrastructure.Data.Documents.BaseDocument;

namespace CatalogService.Infrastructure.Data.Documents
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
    }
}