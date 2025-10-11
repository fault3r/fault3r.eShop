using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.Mediator.Queries;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Handlers.Queries
{
    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, (int Code, IEnumerable<ItemDto> Items)>
    {
        private readonly IItemsService _ItemsService;

        public GetItemsQueryHandler(IItemsService ItemsService)
        {
            _ItemsService = ItemsService;
        }

        public async Task<(int Code, IEnumerable<ItemDto> Items)> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
           return await _ItemsService.GetAllAsync();
        }
    }
}