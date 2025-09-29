using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<(bool Success, string Message, IEnumerable<ItemDto> Items)> GetAllAsync();

        Task<(bool Success, string Message, ItemDto? Item)> GetByIdAsync(string id);

        Task<(bool Success, string Message, ItemDto? Item)> CreateAsync(CreateItemDto item);

        Task<(bool Success, string Message, ItemDto? Item)> UpdateAsync(string id, UpdateItemDto item);

        Task<(bool Success, string Message)> DeleteAsync(string id);
    }
}