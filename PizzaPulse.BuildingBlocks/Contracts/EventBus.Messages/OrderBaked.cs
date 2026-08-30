namespace PizzaPulse.BuildingBlocks.Contracts.EventBus.Messages;

public record OrderBaked(Guid OrderId, string DeliveryAddress, DateTime BakedAt);
