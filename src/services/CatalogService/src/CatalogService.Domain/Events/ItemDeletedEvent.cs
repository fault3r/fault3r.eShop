
using System;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Domain.Events
{
    public class ItemDeletedEvent : IEvent
    {
        public required string Id { get; set; }
        
    }
}