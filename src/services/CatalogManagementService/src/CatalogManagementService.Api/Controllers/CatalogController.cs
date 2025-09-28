
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogManagementService.Api.Controllers
{
    [Route("api/v1/catalog")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CatalogController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

    }
}
