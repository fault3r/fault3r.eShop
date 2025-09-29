using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Queries;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Queries
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, IEnumerable<ItemDto>>
    {
        private readonly ICatalogService _catalogService;

        public GetAllQueryHandler(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<IEnumerable<ItemDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var (Success, Message, Items) = await _catalogService.GetAllAsync();
            return Items;
        }
    }
}