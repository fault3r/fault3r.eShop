using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Queries;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Queries
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, (int Code, IEnumerable<ItemDto> Items)>
    {
        private readonly ICatalogService _catalogService;

        public GetAllQueryHandler(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<(int Code, IEnumerable<ItemDto> Items)> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
           return await _catalogService.GetAllAsync();
        }
    }
}