using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<(string Message, IEnumerable<ItemDto> Items)> GetAllAsync();

        Task<(string Message,ItemDto? Item)> GetByIdAsync(string id);

        Task<(string Message,ItemDto? Item)> CreateAsync(CreateItemDto item);

        Task<(string Message,ItemDto? Item)> UpdateAsync(string id, UpdateItemDto item);

        Task<(string Message, bool Success)> DeleteAsync(string id);
    }
}