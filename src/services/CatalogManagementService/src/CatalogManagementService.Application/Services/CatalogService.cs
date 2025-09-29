using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.Services
{
    public class CatalogService(ICatalogRepository catalogRepository) : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository = catalogRepository;

        public async Task<(bool Success, string Message, IEnumerable<ItemDto> Items)> GetAllAsync()
        {
            var result = await _catalogRepository.GetAllAsync();
            return (
                    Success: result.Success,
                    Message: result.Message,
                    Items: result.Items.Select(item => ItemDTOs.ToDto(item)));
        }

        public async Task<(bool Success, string Message, ItemDto? Item)> GetByIdAsync(string id)
        {
            var result = await _catalogRepository.GetByIdAsync(id);
            return (
                Success: result.Success,                
                Message: result.Message,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<(bool Success, string Message, ItemDto? Item)> CreateAsync(CreateItemDto item)
        {
            var result = await _catalogRepository.CreateAsync(new Item
            {
                Id = nameof(Item),
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Pictures = item.Pictures,
            });
            return (
                Success: result.Success,                                   
                Message: result.Message,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<(bool Success, string Message, ItemDto? Item)> UpdateAsync(string id, UpdateItemDto item)
        {
            var result = await _catalogRepository.UpdateAsync(new Item
            {
                Id = id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Pictures = item.Pictures,
            });
            return (
                Success: result.Success,
                Message: result.Message,
                Item: result.Items.Select(item => ItemDTOs.ToDto(item)).FirstOrDefault());
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            var result = await _catalogRepository.DeleteAsync(id);
            return (
                Success: result.Success,
                Message: result.Message);
        }
    }
}