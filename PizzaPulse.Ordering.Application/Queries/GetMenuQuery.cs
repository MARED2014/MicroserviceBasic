using MediatR;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Application.Queries;

public record GetMenuQuery(Guid Id) : IRequest<PizzaMenu?>;
