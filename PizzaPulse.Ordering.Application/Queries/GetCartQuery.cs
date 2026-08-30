using MediatR;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Application.Queries;

public record GetCartQuery(string CustomerId) : IRequest<IReadOnlyList<CartItem>>;
