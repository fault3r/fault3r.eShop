
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.Services
{
    public class ItemsService(
        IRepository MongoRepository, IEventPublisher eventPublisher) : IItemsService
    {
        private readonly IRepository _MongoRepository = MongoRepository;

        private readonly IEventPublisher _eventPublisher = eventPublisher;

        public async Task<(int Code, IEnumerable<ItemDto> Items)> GetAllAsync()
        {
            var result = await _MongoRepository.GetAllAsync();
            return (
                Code: result.Code,
                Items: result.Items.Select(item => ItemDTOs.ToDto(item)));
        }

        public async Task<(int Code, ItemDto? Item)> GetByIdAsync(string id)
        {
            var result = await _MongoRepository.GetByIdAsync(id);
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<(int Code, ItemDto? Item)> CreateAsync(CreateItemDto item)
        {
            var result = await _MongoRepository.CreateAsync(new Item
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
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<(int Code, ItemDto? Item)> UpdateAsync(string id, UpdateItemDto item)
        {
            var result = await _MongoRepository.UpdateAsync(new Item
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

        public async Task<int> DeleteAsync(string id)
        {
            var result = await _MongoRepository.DeleteAsync(id);
            if (result.Code == (int)MongoRepositoryResultCode.NoContent)
                _eventPublisher.Publish<ItemDeletedEvent>(
                    new ItemDeletedEvent { Id = id });
            return result.Code;
        }

    }
}