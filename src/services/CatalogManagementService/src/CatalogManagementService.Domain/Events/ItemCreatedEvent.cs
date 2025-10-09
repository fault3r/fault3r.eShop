
using System;
using CatalogManagementService.Domain.Events.BaseEvent;

namespace CatalogManagementService.Domain.Events
{
    public class ItemCreatedEvent : ItemEvent
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime Updated { get; set; }

    }
}