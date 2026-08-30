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
    public string CustomerId { get; set; } = "cust-001";
    public List<MenuSelectionItem> Items { get; set; } = [];
}

public class CartPageViewModel
{
    public string CustomerId { get; set; } = "cust-001";
    public List<CartItemDto> Items { get; set; } = [];
    public decimal Total => Items.Sum(i => i.LineTotal);
}

public class AddToCartRequest
{
    public string CustomerId { get; set; } = "cust-001";
    public Guid PizzaMenuId { get; set; }
    public int Quantity { get; set; } = 1;
    public string Size { get; set; } = "Medium";
}
