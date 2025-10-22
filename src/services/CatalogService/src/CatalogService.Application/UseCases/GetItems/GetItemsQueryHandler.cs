
using System;
using CatalogService.Application.DTOs;
using CatalogService.Application.Interfaces;
using MediatR;

namespace CatalogService.Application.UseCases.GetItems
{
    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, (int Code, IEnumerable<ItemDto> Items)>
    {
        private readonly IGetItemsService _service;

        private readonly ILoggerService<GetItemsQueryHandler> _logger;

        public GetItemsQueryHandler(IGetItemsService service,
            ILoggerService<GetItemsQueryHandler> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, IEnumerable<ItemDto> Items)> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            await _logger.LogInformation("forwarding request to service..");
            var (Code, Items) = await _service.ExecuteAsync();
            await _logger.LogInformation("retrieved response from service.");
            return (Code, Items);
        }
    }
}