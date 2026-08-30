using MediatR;

namespace PizzaPulse.Kitchen.Application.Commands;

public record StartOvenCommand(Guid OrderId) : IRequest;
