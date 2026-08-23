using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Ordering.Core.Entities;

// Redis içinde "cart:{customerId}" anahtarıyla JSON olarak saklanır
public class CartItem
{
    public Guid PizzaMenuId { get; set; }
    public string PizzaName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Size { get; set; } = "Medium";
    public decimal UnitPrice { get; set; }
}
