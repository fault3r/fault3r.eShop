
using System;
using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Domain.Events.BaseEvent
{
    public class ItemEvent
    {
        public required string Id { get; set; }

        public static ItemCreatedEvent ToCreateDto(Item item) => new()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
        };
    }
}