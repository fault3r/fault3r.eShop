using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.Services
{
    public class ItemsService(IItemsRepository ItemsRepository) : IItemsService
    {
        private readonly IItemsRepository _ItemsRepository = ItemsRepository;

        public async Task<(int Code, IEnumerable<ItemDto> Items)> GetAllAsync()
        {
            var result = await _ItemsRepository.GetAllAsync();
            return (
                Code: result.Code,
                Items: result.Items.Select(item => ItemDTOs.ToDto(item)));
        }

        public async Task<(int Code, ItemDto? Item)> GetByIdAsync(string id)
        {
            var result = await _ItemsRepository.GetByIdAsync(id);
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<(int Code, ItemDto? Item)> CreateAsync(CreateItemDto item)
        {
            var result = await _ItemsRepository.CreateAsync(new Item
            {
                Id = nameof(Item),
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Pictures = item.Pictures,
            });
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<(int Code, ItemDto? Item)> UpdateAsync(string id, UpdateItemDto item)
        {
            var result = await _ItemsRepository.UpdateAsync(new Item
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
            var result = await _ItemsRepository.DeleteAsync(id);
            return result.Code;
        }
        
    }
}