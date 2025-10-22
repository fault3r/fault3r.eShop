
using System;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.DTOs
{
    public record ItemDto(
        string Id,
        string Name,
        string Description,
        decimal Price,
        DateTime Updated);

    public class ItemDTOs
    {
        public static ItemDto Parse(Item item)
        {
            return new ItemDto(
                item.Id,
                item.Name,
                item.Description,
                item.Price,
                item.Updated);
        }
    }
    
}