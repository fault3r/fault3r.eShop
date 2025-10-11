
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.UseCases.UpdateItem
{
    public class UpdateItemService(
        IRepository repository, IEventPublisher eventPublisher)
    {
        private readonly IRepository _repository = repository;
        
        private readonly IEventPublisher _eventPublisher = eventPublisher;

        public async Task<(int Code, ItemDto? Item)> ExecuteAsync(string id, UpdateItemDto item)
        {
            var result = await _repository.UpdateAsync(new Item
            {
                Id = id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
            });
            if (result.Code == (int)MongoRepositoryResultCode.Ok)
                _eventPublisher.Publish<ItemUpdatedEvent>(
                    ItemUpdatedEvent.Parse(result.Items.First()));
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }
    }
}