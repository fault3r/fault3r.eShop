
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.UseCases.CreateItem
{
    public class CreateItemService(
        IRepository repository, IEventPublisher eventPublisher) : ICreateItemService
    {
        private readonly IRepository _repository = repository;
        
        private readonly IEventPublisher _eventPublisher = eventPublisher;

        public async Task<(int Code, ItemDto? Item)> ExecuteAsync(CreateItemDto item)
        {
            //log
            Console.WriteLine($"{nameof(CreateItemService)} is executing.");            
            var result = await _repository.CreateAsync(new Item
            {
                Id = nameof(Item),
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
            });
            if (result.Code == (int)MongoRepositoryResultCode.Created)
                _eventPublisher.Publish<ItemCreatedEvent>(
                    ItemCreatedEvent.Parse(result.Items.First()));
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.Parse(item)).FirstOrDefault());
        }
    }
}