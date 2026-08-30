using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Application.Queries;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Api.Controllers;

[ApiController]
[Route("api/menu")]
[Tags("Menu")]
public class MenuController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenuController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PizzaMenu>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PizzaMenu>>> GetMenu(CancellationToken cancellationToken)
    {
        var menu = await _mediator.Send(new GetMenuListQuery(), cancellationToken);
        return Ok(menu);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PizzaMenu), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PizzaMenu>> GetMenuItem(Guid id, CancellationToken cancellationToken)
    {
        var item = await _mediator.Send(new GetMenuQuery(id), cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateMenuItem([FromBody] Contracts.CreatePizzaMenuRequest request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(new CreatePizzaMenuCommand(request.Name, request.Description, request.BasePrice, request.IsAvailable),cancellationToken);

        return CreatedAtAction(nameof(GetMenuItem), new { id }, new { id });
    }
}
