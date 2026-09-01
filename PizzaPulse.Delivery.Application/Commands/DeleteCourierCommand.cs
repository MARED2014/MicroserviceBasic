using MediatR;

namespace PizzaPulse.Delivery.Application.Commands;

public record DeleteCourierCommand(Guid Id) : IRequest;
