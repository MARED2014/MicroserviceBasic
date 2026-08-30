namespace PizzaPulse.BuildingBlocks.Contracts.EventBus.Messages;

public record OrderOnTheWay(Guid OrderId, Guid CourierId, string CourierName, DateTime PickedUpAt);
