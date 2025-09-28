using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Commands;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Commands
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemDto?>
    {
        private readonly ICatalogService _catalogService;

        public UpdateItemCommandHandler( ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<ItemDto?> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var result = await _catalogService.UpdateAsync(request.Id, request.Item);
            return result.Item;
        }
    }
}