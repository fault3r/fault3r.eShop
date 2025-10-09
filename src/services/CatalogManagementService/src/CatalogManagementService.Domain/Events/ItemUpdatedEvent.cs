
using System;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Events.BaseEvent;

namespace CatalogManagementService.Domain.Events
{
    public class ItemUpdatedEvent : ItemEvent
    {
        public override string EventType => nameof(ItemUpdatedEvent);

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