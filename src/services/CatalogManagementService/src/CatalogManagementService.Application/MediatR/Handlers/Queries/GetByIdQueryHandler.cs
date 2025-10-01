using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Queries;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Queries
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, (int Code, ItemDto? Item)>
    {
        private readonly ICatalogService _catalogService;

        public GetByIdQueryHandler(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _catalogService.GetByIdAsync(request.Id);
        }
    }
}