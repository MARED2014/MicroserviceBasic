using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Ordering.Core.Entities;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid PizzaMenuId { get; set; }
    public string PizzaName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Size { get; set; } = "Medium"; // Small, Medium, Large
}
