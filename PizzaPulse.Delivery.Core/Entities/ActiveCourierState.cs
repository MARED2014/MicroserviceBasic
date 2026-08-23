using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Delivery.Core.Entities;

// Redis içinde "courier:active:{courierId}" anahtarıyla hızlı eşleşme için tutulur
public class ActiveCourierState
{
    public Guid CourierId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public bool IsBusy { get; set; } // True ise yolda, False ise restoranda yeni sipariş bekliyor
    public DateTime LastStatusUpdate { get; set; } = DateTime.UtcNow;
}
