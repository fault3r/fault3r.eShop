using System;

namespace CatalogManagementService.Application.DTOs
{
    public record ItemDto(
        string Id,
        string Name,
        string Description,
        decimal Price,
        IEnumerable<string> Pictures,
        DateTime Updated
    );
}
