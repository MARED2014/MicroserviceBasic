using MediatR;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Application.Queries;

public record GetOrderQuery(Guid Id) : IRequest<Order?>;
