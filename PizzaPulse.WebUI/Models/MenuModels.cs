namespace PizzaPulse.WebUI.Models;

public class PizzaMenuDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public bool IsAvailable { get; set; }
}

public class CartItemDto
{
    public Guid PizzaMenuId { get; set; }
    public string PizzaName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Size { get; set; } = "Medium";
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}

public class MenuSelectionItem
{
    public bool Selected { get; set; }
    public Guid PizzaMenuId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public int Quantity { get; set; } = 1;
    public string Size { get; set; } = "Medium";
}

public class MenuPageViewModel
{
    public string CustomerId { get; set; } = string.Empty;
    public List<MenuSelectionItem> Items { get; set; } = [];
}

public class CartPageViewModel
{
    public string CustomerId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = [];
    public decimal Total => Items.Sum(i => i.LineTotal);
    public string CustomerName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
}

public class PlaceOrderRequest
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
}

public class PlaceOrderResponse
{
    public Guid OrderId { get; set; }
}

public class OrderItemDto
{
    public Guid PizzaMenuId { get; set; }
    public string PizzaName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Size { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; } = [];

    public string StatusText => Status switch
    {
        1 => "Alındı",
        2 => "Hazırlanıyor",
        3 => "Pişti",
        4 => "Yolda",
        5 => "Teslim edildi",
        6 => "İptal",
        _ => "Bilinmiyor"
    };
}

public class AddToCartRequest
{
    public string CustomerId { get; set; } = string.Empty;
    public Guid PizzaMenuId { get; set; }
    public int Quantity { get; set; } = 1;
    public string Size { get; set; } = "Medium";
}
