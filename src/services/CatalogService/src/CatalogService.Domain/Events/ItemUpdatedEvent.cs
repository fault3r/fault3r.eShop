
using System;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Domain.Events
{
    public class ItemUpdatedEvent : IEvent
    {
        public required string Id { get; set; }
        
        public required string Name { get; set; }

        public required string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime Updated { get; set; }

        public static ItemUpdatedEvent Parse(Item item) => new()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            Updated = item.Updated,
        };
    }
}