using MediatR;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Application.Commands;

public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus Status) : IRequest;
