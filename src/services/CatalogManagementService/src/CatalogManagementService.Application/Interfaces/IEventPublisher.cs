
using System;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IEventPublisher
    {
        bool Publish<TEvent>(TEvent @event) where TEvent : IEvent;

    }
}