
using System;
using MediatR;

namespace CatalogManagementService.Application.UseCases.DeleteItem
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, int>
    {
        private readonly DeleteItemService _service;

        public DeleteItemCommandHandler(DeleteItemService service)
        {
            _service = service;
        }

        public async Task<int> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            return await _service.ExecuteAsync(request.Id);
        }

    }
}