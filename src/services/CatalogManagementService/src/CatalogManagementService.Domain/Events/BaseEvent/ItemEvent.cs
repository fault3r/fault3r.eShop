
using System;

namespace CatalogManagementService.Domain.Events.BaseEvent
{
    public abstract class ItemEvent
    {
        public required string Id { get; set; }

    }
}