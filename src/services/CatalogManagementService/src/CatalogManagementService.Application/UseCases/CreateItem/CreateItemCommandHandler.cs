
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using MediatR;

namespace CatalogManagementService.Application.UseCases.CreateItem
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, (int Code, ItemDto? Item)>
    {
        private readonly ICreateItemService _service;

        public CreateItemCommandHandler(ICreateItemService service)
        {
            _service = service;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            //log
            Console.WriteLine($"{nameof(CreateItemCommandHandler)} received a request.");
            return await _service.ExecuteAsync(request.Item);
        }
    }
}