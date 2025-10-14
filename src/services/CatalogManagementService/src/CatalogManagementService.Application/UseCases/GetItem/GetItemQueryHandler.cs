
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using MediatR;

namespace CatalogManagementService.Application.UseCases.GetItem
{
    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, (int Code, ItemDto? Item)>
    {
        private readonly IGetItemService _service;

        public GetItemQueryHandler(IGetItemService service)
        {
            _service = service;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            return await _service.ExecuteAsync(request.Id);
        }

    }
}