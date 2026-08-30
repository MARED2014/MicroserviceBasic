namespace PizzaPulse.Contracts.Events;

public record OrderOnTheWay(Guid OrderId, Guid CourierId, string CourierName, DateTime PickedUpAt);
