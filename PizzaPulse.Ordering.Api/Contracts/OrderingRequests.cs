using System.ComponentModel;

namespace PizzaPulse.Ordering.Api.Contracts;

public class CreatePizzaMenuRequest
{
    [DefaultValue("Diavola")]
    public string Name { get; set; } = "Diavola";

    [DefaultValue("Acılı salam, mozzarella, pul biber")]
    public string Description { get; set; } = "Acılı salam, mozzarella, pul biber";

    [DefaultValue(250)]
    public decimal BasePrice { get; set; } = 250m;

    [DefaultValue(true)]
    public bool IsAvailable { get; set; } = true;
}

public class AddToCartRequest
{
    [DefaultValue(DemoData.CustomerId)]
    public string CustomerId { get; set; } = DemoData.CustomerId;

    public Guid PizzaMenuId { get; set; } = DemoData.MargheritaId;

    [DefaultValue(1)]
    public int Quantity { get; set; } = 1;

    [DefaultValue("Medium")]
    public string Size { get; set; } = "Medium";
}

public class PlaceOrderRequest
{
    [DefaultValue(DemoData.CustomerId)]
    public string CustomerId { get; set; } = DemoData.CustomerId;

    [DefaultValue(DemoData.CustomerName)]
    public string CustomerName { get; set; } = DemoData.CustomerName;

    [DefaultValue(DemoData.DeliveryAddress)]
    public string DeliveryAddress { get; set; } = DemoData.DeliveryAddress;
}
