using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.Services
{
    public class CatalogService(ICatalogRepository catalogRepository) : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository = catalogRepository;

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _catalogRepository.GetAllAsync();
            return items.Select(item => ItemDTOs.ToDto(item));
        }

        public async Task<(bool Success, ItemDto? Item)> GetByIdAsync(string id)
        {
            var result = await _catalogRepository.GetByIdAsync(id);
            if (result.Item is null)
                return (Success: false, Item: null);
            return (Success: true, Item: ItemDTOs.ToDto(result.Item));
        }

        public async Task<(bool Success, ItemDto? Item)> CreateAsync(CreateItemDto item)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool Success, ItemDto? Item)> UpdateAsync(UpdateItemDto item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}