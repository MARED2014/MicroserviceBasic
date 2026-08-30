using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Application.Consumers;

public class OrderBakedConsumer : IConsumer<OrderBaked>
{
    private readonly IMediator _mediator;

    public OrderBakedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<OrderBaked> context)
    {
        return _mediator.Send(new UpdateOrderStatusCommand(context.Message.OrderId, OrderStatus.Baked), context.CancellationToken);
    }
}
