using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Commands;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Commands
{
    public class CreateCommandHandler : IRequestHandler<CreateItemCommand, ItemDto?>
    {
        private readonly ICatalogService _catalogService;

        public CreateCommandHandler(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<ItemDto?> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var result = await _catalogService.CreateAsync(request.Item);
            return result.Item;
        }
    }
}