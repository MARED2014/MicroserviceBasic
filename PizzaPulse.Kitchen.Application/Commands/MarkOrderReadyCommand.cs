using MediatR;

namespace PizzaPulse.Kitchen.Application.Commands;

public record MarkOrderReadyCommand(Guid OrderId) : IRequest;
