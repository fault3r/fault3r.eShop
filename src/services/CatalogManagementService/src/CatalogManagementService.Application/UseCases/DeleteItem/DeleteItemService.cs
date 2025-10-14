
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.UseCases.DeleteItem
{
    public class DeleteItemService(
        IRepository repository, IEventPublisher eventPublisher) : IDeleteItemService
    {
        private readonly IRepository _repository = repository;
        
        private readonly IEventPublisher _eventPublisher = eventPublisher;

        public async Task<int> ExecuteAsync(string id)
        {
            var result = await _repository.DeleteAsync(id);
            if (result.Code == (int)MongoRepositoryResultCode.NoContent)
                _eventPublisher.Publish<ItemDeletedEvent>(
                    new ItemDeletedEvent { Id = id });
            return result.Code;
        }

    }
}