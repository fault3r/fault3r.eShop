using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Commands;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Commands
{
    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ItemDto?>
    {
        private readonly ICatalogService _catalogService;

        public UpdateCommandHandler( ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<ItemDto?> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var (Success, Message, Item) = await _catalogService.UpdateAsync(request.Id, request.Item);
            return Item;
        }
    }
}