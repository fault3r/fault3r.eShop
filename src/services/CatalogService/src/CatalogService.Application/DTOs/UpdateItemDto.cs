
using System;

namespace CatalogService.Application.DTOs
{
    public record UpdateItemDto(
        string Name,
        string Description,
        decimal Price);

}