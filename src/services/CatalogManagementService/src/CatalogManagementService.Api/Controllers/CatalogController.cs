
using System;
using System.Text.Json;
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
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAllAsync()
        {
            var items = await _mediator.Send(new GetAllQuery());
            return Ok(items);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ItemDto?>> GetByIdAsync([FromRoute] string id)
        {
            var item = await _mediator.Send(new GetByIdQuery { Id = id });
            if (item is null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto?>> CreateAsync([FromBody] CreateItemDto item)
        {
            var createdItem = await _mediator.Send(new CreateCommand { Item = item });
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateItemDto item)
        {
            var updatedItem = await _mediator.Send(new UpdateCommand { Id = id, Item = item });
            if (updatedItem is null)
                return NotFound();
            return Ok(updatedItem);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] string id)
        {
            bool result = await _mediator.Send(new DeleteCommand { Id = id });
            if (!result)
                return NotFound();
            return NoContent();
        }
        
    }
}
