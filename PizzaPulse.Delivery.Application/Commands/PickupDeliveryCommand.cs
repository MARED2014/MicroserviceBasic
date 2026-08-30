using MediatR;

namespace PizzaPulse.Delivery.Application.Commands;

public record PickupDeliveryCommand(Guid OrderId) : IRequest;
