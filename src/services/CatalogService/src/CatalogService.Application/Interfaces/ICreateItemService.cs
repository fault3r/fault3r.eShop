
using System;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Interfaces
{
    public interface ICreateItemService
    {
        Task<(int Code, ItemDto? Item)> ExecuteAsync(CreateItemDto item);
    }
}
