
using System;
using CatalogService.Application.DTOs;
using CatalogService.Application.Interfaces;
using MediatR;

namespace CatalogService.Application.UseCases.GetItem
{
    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, (int Code, ItemDto? Item)>
    {
        private readonly IGetItemService _service;

        private readonly ILoggerService<GetItemQueryHandler> _logger;

        public GetItemQueryHandler(IGetItemService service,
            ILoggerService<GetItemQueryHandler> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, ItemDto? Item)> Handle(GetItemQuery request, CancellationToken ct)
        {
            await _logger.LogInformation("forwarding request to service..");
            var (Code, Item) = await _service.ExecuteAsync(request.Id);
            await _logger.LogInformation("retrieved response from service.");
            return (Code, Item);
        }
    }
}