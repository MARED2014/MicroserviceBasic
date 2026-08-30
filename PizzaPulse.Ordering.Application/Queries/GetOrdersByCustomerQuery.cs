using MediatR;
using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Application.Queries;

public record GetOrdersByCustomerQuery(string CustomerId) : IRequest<IEnumerable<Order>>;
