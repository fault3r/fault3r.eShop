
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.MediatR.Commands;
using CatalogManagementService.Application.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogManagementService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/catalog")]
    [ApiVersion("1.0")]
    public class CatalogController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
        {
            var items = await _mediator.Send(new GetAllQuery());
            return Ok(items);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ItemDto?>> GetById([FromRoute] string id)
        {
            var item = await _mediator.Send(new GetByIdQuery { Id = id });
            if (item is null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto?>> Create([FromBody] CreateItemDto item)
        {
            var newItem = await _mediator.Send(new CreateCommand { Item = item });
            return Ok();

        }
    }
}
