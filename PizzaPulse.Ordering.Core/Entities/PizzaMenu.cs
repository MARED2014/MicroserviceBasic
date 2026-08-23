using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Ordering.Core.Entities;

public class PizzaMenu
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; // Örn: Pepperoni Supreme
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
