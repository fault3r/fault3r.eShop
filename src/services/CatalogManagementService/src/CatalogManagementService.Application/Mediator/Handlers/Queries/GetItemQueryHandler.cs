using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.Mediator.Queries;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Handlers.Queries
{
    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, (int Code, ItemDto? Item)>
    {
        private readonly IItemsService _ItemsService;

        public GetItemQueryHandler(IItemsService ItemsService)
        {
            _ItemsService = ItemsService;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            return await _ItemsService.GetByIdAsync(request.Id);
        }
    }
}