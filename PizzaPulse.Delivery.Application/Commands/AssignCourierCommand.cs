using MediatR;

namespace PizzaPulse.Delivery.Application.Commands;

public record AssignCourierCommand(Guid OrderId, string CustomerAddress) : IRequest<Guid>;
