using System;

namespace CatalogService.Application.DTOs
{
    public class CatalogDTOs
    {
        public record ItemDto(
            string Id,
            string Name,
            string Description,
            decimal Price,
            ICollection<string> Pictures,
            DateTime Updated
        );

        public record CreateItemDto(
            string Name,
            string Description,
            decimal Price,
            ICollection<string> Pictures
        );

        public record UpdateItemDto(
            string Name,
            string Description,
            decimal Price,
            ICollection<string> Pictures
        );          
    }
}