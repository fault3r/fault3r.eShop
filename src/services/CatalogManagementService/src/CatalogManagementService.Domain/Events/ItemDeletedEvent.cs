
using System;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Domain.Events
{
    public class ItemDeletedEvent : IEvent
    {
        public required string Id { get; set; }
        
    }
}