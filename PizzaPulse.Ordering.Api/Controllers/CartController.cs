using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPulse.Ordering.Api.Contracts;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Application.Queries;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Api.Controllers;

[ApiController]
[Route("api/cart")]
[Tags("Cart")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CartItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CartItem>>> GetCart(
        [FromQuery] string customerId = DemoData.CustomerId,
        CancellationToken cancellationToken = default)
    {
        var cart = await _mediator.Send(new GetCartQuery(customerId), cancellationToken);
        return Ok(cart);
    }

    [HttpPost("items")]
    [ProducesResponseType(typeof(IReadOnlyList<CartItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<CartItem>>> AddItem(
        [FromBody] AddToCartRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(
                new AddToCartCommand(request.CustomerId, request.PizzaMenuId, request.Quantity, request.Size),
                cancellationToken);

            var cart = await _mediator.Send(new GetCartQuery(request.CustomerId), cancellationToken);
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
