
using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.UseCases.GetItem
{
    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, (int Code, ItemDto? Item)>
    {
        private readonly GetItemService _service;

        public GetItemQueryHandler(GetItemService service)
        {
            _service = service;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            return await _service.ExecuteAsync(request.Id);
        }

    }
}