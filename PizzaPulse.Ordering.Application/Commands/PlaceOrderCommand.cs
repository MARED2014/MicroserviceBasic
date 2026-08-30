using MediatR;

namespace PizzaPulse.Ordering.Application.Commands;

public record PlaceOrderCommand(string CustomerId, string CustomerName, string DeliveryAddress) : IRequest<Guid>;
