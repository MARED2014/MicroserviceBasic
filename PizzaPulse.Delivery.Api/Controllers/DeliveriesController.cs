using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPulse.Delivery.Api.Contracts;
using PizzaPulse.Delivery.Application.Commands;
using PizzaPulse.Delivery.Application.Queries;
using PizzaPulse.Delivery.Core.Entities;

namespace PizzaPulse.Delivery.Api.Controllers;

[ApiController]
[Route("api/deliveries")]
[Tags("Delivery")]
public class DeliveriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(DeliveryAssignment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeliveryAssignment>> GetByOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var assignment = await _mediator.Send(new GetDeliveryAssignmentQuery(orderId), cancellationToken);
        return assignment is null ? NotFound() : Ok(assignment);
    }

    [HttpPost("assign")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Assign([FromBody] AssignCourierRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _mediator.Send(
                new AssignCourierCommand(request.OrderId, request.CustomerAddress),
                cancellationToken);

            return CreatedAtAction(nameof(GetByOrder), new { orderId = request.OrderId }, new { id, request.OrderId });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{orderId:guid}/pickup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Pickup(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new PickupDeliveryCommand(orderId), cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{orderId:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Complete(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new CompleteDeliveryCommand(orderId), cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
