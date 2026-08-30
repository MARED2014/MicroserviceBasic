using MediatR;

namespace PizzaPulse.Delivery.Application.Commands;

public record CompleteDeliveryCommand(Guid OrderId) : IRequest;
