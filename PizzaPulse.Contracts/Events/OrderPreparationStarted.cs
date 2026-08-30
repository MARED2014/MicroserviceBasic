namespace PizzaPulse.Contracts.Events;

public record OrderPreparationStarted(Guid OrderId, DateTime StartedAt);
