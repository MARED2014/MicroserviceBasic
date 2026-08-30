using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPulse.Delivery.Application.Queries;
using PizzaPulse.Delivery.Core.Entities;

namespace PizzaPulse.Delivery.Api.Controllers;

[ApiController]
[Route("api/couriers")]
[Tags("Couriers")]
public class CouriersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CouriersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<Courier>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Courier>>> GetActive(CancellationToken cancellationToken)
    {
        var couriers = await _mediator.Send(new GetActiveCouriersQuery(), cancellationToken);
        return Ok(couriers);
    }
}
