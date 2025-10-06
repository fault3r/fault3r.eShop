using System;
using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Domain.Events
{
    public class ItemCreatedEvent
    {
        public required Item Item { get; set; }
    }
}