
using System;
using CatalogManagementService.Domain.Events.BaseEvent;

namespace CatalogManagementService.Domain.Events
{
    public class ItemDeletedEvent : ItemEvent
    {
        public required string Id { get; set; }
        
    }
}