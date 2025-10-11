
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.UseCases.CreateItem;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Handlers.Commands
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, (int Code, ItemDto? Item)>
    {
        private readonly CreateItemService _service;

        public CreateItemCommandHandler(CreateItemService service)
        {
            _service = service;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            return await _service.ExecuteAsync(request.Item);
        }
        
    }
}