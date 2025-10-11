
using System;
using CatalogManagementService.Domain.Events.BaseEvent;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IEventPublisher
    {
        bool Publish<TEvent>(TEvent @event) where TEvent : ItemEvent;

    }
}