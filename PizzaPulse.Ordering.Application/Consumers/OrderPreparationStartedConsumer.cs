using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Application.Consumers;

public class OrderPreparationStartedConsumer : IConsumer<OrderPreparationStarted>
{
    private readonly IMediator _mediator;

    public OrderPreparationStartedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<OrderPreparationStarted> context)
    {
        return _mediator.Send(new UpdateOrderStatusCommand(context.Message.OrderId, OrderStatus.Preparing), context.CancellationToken);
    }
}
