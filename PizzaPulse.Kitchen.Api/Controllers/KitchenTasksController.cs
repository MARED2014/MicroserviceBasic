using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPulse.Kitchen.Api.Contracts;
using PizzaPulse.Kitchen.Application.Commands;
using PizzaPulse.Kitchen.Application.Queries;
using PizzaPulse.Kitchen.Core.Entities;

namespace PizzaPulse.Kitchen.Api.Controllers;

[ApiController]
[Route("api/kitchen/tasks")]
[Tags("Kitchen")]
public class KitchenTasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public KitchenTasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<KitchenTask>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<KitchenTask>>> GetPending(CancellationToken cancellationToken)
    {
        var tasks = await _mediator.Send(new GetPendingKitchenTasksQuery(), cancellationToken);
        return Ok(tasks);
    }

    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(KitchenTask), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KitchenTask>> GetByOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var task = await _mediator.Send(new GetKitchenTaskQuery(orderId), cancellationToken);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateKitchenTaskRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            new CreateKitchenTaskCommand(request.OrderId, request.ItemsSummary, request.DeliveryAddress),
            cancellationToken);

        return CreatedAtAction(nameof(GetByOrder), new { orderId = request.OrderId }, new { id, request.OrderId });
    }

    [HttpPost("{orderId:guid}/start-oven")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartOven(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new StartOvenCommand(orderId), cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{orderId:guid}/ready")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkReady(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new MarkOrderReadyCommand(orderId), cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
