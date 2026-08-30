namespace PizzaPulse.BuildingBlocks.Contracts.EventBus.Messages;

public record OrderDelivered(Guid OrderId, DateTime DeliveredAt);