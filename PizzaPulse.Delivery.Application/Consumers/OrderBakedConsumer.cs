using MassTransit;
using MediatR;
using PizzaPulse.BuildingBlocks.Contracts.EventBus.Messages;
using PizzaPulse.Delivery.Application.Commands;

namespace PizzaPulse.Delivery.Application.Consumers;

public class OrderBakedConsumer : IConsumer<OrderBaked>
{
    private readonly IMediator _mediator;

    public OrderBakedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<OrderBaked> context)
    {
        return _mediator.Send(new AssignCourierCommand(context.Message.OrderId, context.Message.DeliveryAddress),context.CancellationToken);
    }
}
