namespace PizzaPulse.BuildingBlocks.Contracts.EventBus.Messages;

public record OrderPreparationStarted(Guid OrderId, DateTime StartedAt);
