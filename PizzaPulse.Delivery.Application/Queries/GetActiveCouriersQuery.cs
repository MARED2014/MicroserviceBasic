using MediatR;
using PizzaPulse.Delivery.Core.Entities;

namespace PizzaPulse.Delivery.Application.Queries;

public record GetActiveCouriersQuery() : IRequest<IEnumerable<Courier>>;
