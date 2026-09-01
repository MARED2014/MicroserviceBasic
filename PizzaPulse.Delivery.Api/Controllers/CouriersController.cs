using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPulse.Delivery.Api.Contracts;
using PizzaPulse.Delivery.Application.Commands;
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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Courier>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Courier>>> GetAll(CancellationToken cancellationToken)
    {
        var couriers = await _mediator.Send(new GetCouriersQuery(), cancellationToken);
        return Ok(couriers);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<Courier>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Courier>>> GetActive(CancellationToken cancellationToken)
    {
        var couriers = await _mediator.Send(new GetActiveCouriersQuery(), cancellationToken);
        return Ok(couriers);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Courier), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Courier>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var courier = await _mediator.Send(new GetCourierQuery(id), cancellationToken);
        return courier is null ? NotFound() : Ok(courier);
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCourierRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _mediator.Send(
                new CreateCourierCommand(request.FullName, request.Phone, request.VehiclePlate, request.IsActive),
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourierRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(
                new UpdateCourierCommand(id, request.FullName, request.Phone, request.VehiclePlate, request.IsActive),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new DeleteCourierCommand(id), cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
