using MediatR;

namespace PizzaPulse.Ordering.Application.Commands;

public record AddToCartCommand(string CustomerId, Guid PizzaMenuId, int Quantity, string Size = "Medium") : IRequest;
