
using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.UseCases.UpdateItem
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, (int Code, ItemDto? Item)>
    {
        private readonly UpdateItemService _service;

        public UpdateItemCommandHandler(UpdateItemService service)
        {
            _service = service;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            return await _service.ExecuteAsync(request.Id, request.Item);
        }

    }
}