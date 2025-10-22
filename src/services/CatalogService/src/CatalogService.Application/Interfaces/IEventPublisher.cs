
using System;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task<bool> PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}