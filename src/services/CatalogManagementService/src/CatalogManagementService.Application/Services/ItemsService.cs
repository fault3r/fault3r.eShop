using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.Services
{
    public class ItemsService(IItemsRepository itemsRepository, IEventPublisher eventPublisher) : IItemsService
    {
        private readonly IItemsRepository _itemsRepository = itemsRepository;

        private readonly IEventPublisher _eventPublisher = eventPublisher ;

        public async Task<(int Code, IEnumerable<ItemDto> Items)> GetAllAsync()
        {
            var result = await _itemsRepository.GetAllAsync();
            return (
                Code: result.Code,
                Items: result.Items.Select(item => ItemDTOs.ToDto(item)));
        }

        public async Task<(int Code, ItemDto? Item)> GetByIdAsync(string id)
        {
            var result = await _itemsRepository.GetByIdAsync(id);
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<(int Code, ItemDto? Item)> CreateAsync(CreateItemDto item)
        {
            var result = await _itemsRepository.CreateAsync(new Item
            {
                Id = nameof(Item),
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Pictures = item.Pictures,
            });

            //event
            await _eventPublisher.PublishAsync(new ItemCreatedEvent
            {
                Item = result.Items.FirstOrDefault() ?? throw new InvalidOperationException(),
            });

            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<(int Code, ItemDto? Item)> UpdateAsync(string id, UpdateItemDto item)
        {
            var result = await _itemsRepository.UpdateAsync(new Item
            {
                Id = id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Pictures = item.Pictures,
            });
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = await _itemsRepository.DeleteAsync(id);
            return result.Code;
        }
        
    }
}