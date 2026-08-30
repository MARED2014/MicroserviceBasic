using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Application.Consumers;

public class OrderDeliveredConsumer : IConsumer<OrderDelivered>
{
    private readonly IMediator _mediator;

    public OrderDeliveredConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<OrderDelivered> context)
    {
        return _mediator.Send(new UpdateOrderStatusCommand(context.Message.OrderId, OrderStatus.Delivered), context.CancellationToken);
    }
}
