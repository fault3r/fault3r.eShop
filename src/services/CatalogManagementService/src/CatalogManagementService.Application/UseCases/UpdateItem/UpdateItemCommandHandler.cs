using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.Mediator.Commands;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Handlers.Commands
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, (int Code, ItemDto? Item)>
    {
        private readonly IItemsService _ItemsService;

        public UpdateItemCommandHandler( IItemsService ItemsService)
        {
            _ItemsService = ItemsService;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            return await _ItemsService.UpdateAsync(request.Id, request.Item);
        }
    }
}