using MassTransit;
using MediatR;
using PizzaPulse.Contracts.Events;
using PizzaPulse.Ordering.Application.Commands;
using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;

namespace PizzaPulse.Ordering.Application.Handlers;

public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, Guid>
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderingRepository _orderingRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public PlaceOrderHandler(
        ICartRepository cartRepository,
        IOrderingRepository orderingRepository,
        IPublishEndpoint publishEndpoint)
    {
        _cartRepository = cartRepository;
        _orderingRepository = orderingRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerId)
            || string.IsNullOrWhiteSpace(request.CustomerName)
            || string.IsNullOrWhiteSpace(request.DeliveryAddress))
        {
            throw new InvalidOperationException("Müşteri ve teslimat adresi zorunludur.");
        }

        var cart = await _cartRepository.GetCartAsync(request.CustomerId);
        if (cart.Count == 0)
            throw new InvalidOperationException("Sepet boş. Sipariş oluşturulamadı.");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            DeliveryAddress = request.DeliveryAddress,
            Status = OrderStatus.Received,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var cartItem in cart)
        {
            order.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                PizzaMenuId = cartItem.PizzaMenuId,
                PizzaName = cartItem.PizzaName,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                Size = cartItem.Size
            });
        }

        order.TotalAmount = order.Items.Sum(item => item.UnitPrice * item.Quantity);

        await _orderingRepository.AddAsync(order);
        await _orderingRepository.SaveChangesAsync();
        await _cartRepository.ClearCartAsync(request.CustomerId);

        await _publishEndpoint.Publish(new OrderPlaced(
            order.Id,
            order.CustomerId,
            order.CustomerName,
            order.DeliveryAddress,
            order.Items.Select(item => new OrderPlacedItem(item.PizzaName, item.Quantity, item.Size)).ToList(),
            order.CreatedAt), cancellationToken);

        return order.Id;
    }
}
