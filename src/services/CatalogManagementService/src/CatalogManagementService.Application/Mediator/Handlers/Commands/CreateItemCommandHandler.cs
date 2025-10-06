using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.Mediator.Commands;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Handlers.Commands
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, (int Code, ItemDto? Item)>
    {
        private readonly IItemsService _ItemsService;

        public CreateItemCommandHandler(IItemsService ItemsService)
        {
            _ItemsService = ItemsService;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            return await _ItemsService.CreateAsync(request.Item);
        }
    }
}