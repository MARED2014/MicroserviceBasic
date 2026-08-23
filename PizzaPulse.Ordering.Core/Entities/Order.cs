using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Ordering.Core.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Received; // Received, Preparing, Baked, OnTheWay, Delivered, Cancelled
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relational property
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public enum OrderStatus
{
    Received = 1,
    Preparing = 2,
    Baked = 3,
    OnTheWay = 4,
    Delivered = 5,
    Cancelled = 6
}
