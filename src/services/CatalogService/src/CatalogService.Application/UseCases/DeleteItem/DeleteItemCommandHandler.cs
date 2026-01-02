
using System;
using CatalogService.Application.Interfaces;
using MediatR;

namespace CatalogService.Application.UseCases.DeleteItem
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, int>
    {
        private readonly IDeleteItemService _service;

        private readonly ILoggerService<DeleteItemCommandHandler> _logger;

        public DeleteItemCommandHandler(IDeleteItemService service,
            ILoggerService<DeleteItemCommandHandler> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<int> Handle(DeleteItemCommand request, CancellationToken ct)
        {
            await _logger.LogInformation("forwarding request to service..");
            var result =  await _service.ExecuteAsync(request.Id);
            await _logger.LogInformation("retrieved response from service.");
            return result;
        }
    }
}