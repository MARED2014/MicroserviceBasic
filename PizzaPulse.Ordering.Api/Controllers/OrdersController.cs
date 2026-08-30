using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPulse.Ordering.Api.Contracts;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Application.Queries;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Api.Controllers;

[ApiController]
[Route("api/orders")]
[Tags("Orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> GetOrder(Guid id, CancellationToken cancellationToken)
    {
        var order = await _mediator.Send(new GetOrderQuery(id), cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCustomer(
        [FromQuery] string customerId = DemoData.CustomerId,
        CancellationToken cancellationToken = default)
    {
        var orders = await _mediator.Send(new GetOrdersByCustomerQuery(customerId), cancellationToken);
        return Ok(orders);
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var orderId = await _mediator.Send(
                new PlaceOrderCommand(request.CustomerId, request.CustomerName, request.DeliveryAddress),
                cancellationToken);

            return CreatedAtAction(nameof(GetOrder), new { id = orderId }, new { orderId });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
