
using System;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Interfaces
{
    public interface IUpdateItemService
    {
        Task<(int Code, ItemDto? Item)> ExecuteAsync(string id, UpdateItemDto item);
    }
}
