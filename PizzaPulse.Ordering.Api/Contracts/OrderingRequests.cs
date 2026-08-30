namespace PizzaPulse.Ordering.Api.Contracts;

public class CreatePizzaMenuRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public bool IsAvailable { get; set; } = true;
}

public class AddToCartRequest
{
    public string CustomerId { get; set; } = string.Empty;
    public Guid PizzaMenuId { get; set; }
    public int Quantity { get; set; }
    public string Size { get; set; } = string.Empty;
}

public class PlaceOrderRequest
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
}
