
using System;
using CatalogManagementService.Domain.Events.BaseEvent;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IEventPublisher
    {
        bool Publish<T>(T @event)
            where T : ItemEvent;

    }
}