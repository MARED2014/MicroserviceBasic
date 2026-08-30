using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Application.Consumers;

public class OrderOnTheWayConsumer : IConsumer<OrderOnTheWay>
{
    private readonly IMediator _mediator;

    public OrderOnTheWayConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<OrderOnTheWay> context)
    {
        return _mediator.Send(new UpdateOrderStatusCommand(context.Message.OrderId, OrderStatus.OnTheWay), context.CancellationToken);
    }
}
