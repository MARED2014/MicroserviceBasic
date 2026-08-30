using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Kitchen.Application.Commands;

namespace PizzaPulse.Kitchen.Application.Consumers;

public class OrderPlacedConsumer : IConsumer<OrderPlaced>
{
    private readonly IMediator _mediator;

    public OrderPlacedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        var items = context.Message.Items
            .Select(item => $"{item.Quantity}x {item.Size} {item.PizzaName}")
            .ToList();

        return _mediator.Send(
            new CreateKitchenTaskCommand(context.Message.OrderId, items, context.Message.DeliveryAddress),
            context.CancellationToken);
    }
}
