
using System;
using CatalogManagementService.Application.Interfaces;
using MediatR;

namespace CatalogManagementService.Application.UseCases.DeleteItem
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, int>
    {
        private readonly IDeleteItemService _service;

        public DeleteItemCommandHandler(IDeleteItemService service)
        {
            _service = service;
        }

        public async Task<int> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            return await _service.ExecuteAsync(request.Id);
        }

    }
}