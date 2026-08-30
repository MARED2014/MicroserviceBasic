namespace PizzaPulse.Contracts.Events;

public record OrderPlacedItem(string PizzaName, int Quantity, string Size);

public record OrderPlaced(
    Guid OrderId,
    string CustomerId,
    string CustomerName,
    string DeliveryAddress,
    IReadOnlyList<OrderPlacedItem> Items,
    DateTime PlacedAt);
