using MediatR;

namespace PizzaPulse.Ordering.Application.Commands;

public record CreatePizzaMenuCommand(string Name, string Description, decimal BasePrice, bool IsAvailable) : IRequest<Guid>;