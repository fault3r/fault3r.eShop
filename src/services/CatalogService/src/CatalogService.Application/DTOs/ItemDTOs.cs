
using System;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.DTOs
{
    public class ItemDTOs
    {
        public static ItemDto ToItemDto(Item item)
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
