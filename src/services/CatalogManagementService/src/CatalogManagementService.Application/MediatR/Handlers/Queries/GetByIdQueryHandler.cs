using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Queries;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Queries
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ItemDto?>
    {
        private readonly ICatalogService _catalogService;

        public GetByIdQueryHandler(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<ItemDto?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var (Success, Message, Item) = await _catalogService.GetByIdAsync(request.Id);
            return Item;
        }
    }
}