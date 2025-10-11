
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.UseCases.GetItem;
using MediatR;

namespace CatalogManagementService.Application.UseCases.GetItems
{
    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, (int Code, IEnumerable<ItemDto> Items)>
    {
        private readonly GetItemsService _service;

        public GetItemsQueryHandler(GetItemsService service)
        {
            _service = service;
        }

        public async Task<(int Code, IEnumerable<ItemDto> Items)> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            return await _service.ExecuteAsync();
        }
        
    }
}