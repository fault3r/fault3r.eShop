
using System;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task<bool> PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}