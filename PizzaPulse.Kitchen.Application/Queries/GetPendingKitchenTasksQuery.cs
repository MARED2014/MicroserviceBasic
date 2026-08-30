using MediatR;
using PizzaPulse.Kitchen.Core.Entities;

namespace PizzaPulse.Kitchen.Application.Queries;

public record GetPendingKitchenTasksQuery() : IRequest<IEnumerable<KitchenTask>>;
