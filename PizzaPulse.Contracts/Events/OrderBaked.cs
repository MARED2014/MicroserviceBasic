namespace PizzaPulse.Contracts.Events;

public record OrderBaked(Guid OrderId, string DeliveryAddress, DateTime BakedAt);
