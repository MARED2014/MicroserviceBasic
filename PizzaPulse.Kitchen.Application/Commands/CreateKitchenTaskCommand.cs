using MediatR;

namespace PizzaPulse.Kitchen.Application.Commands;

public record CreateKitchenTaskCommand(Guid OrderId, IReadOnlyList<string> ItemsSummary, string DeliveryAddress) : IRequest<Guid>;
