using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Delivery.Core.Entities;

public class DeliveryAssignment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; } // Order Service referansı
    public Guid CourierId { get; set; } // Courier referansı

    public string CustomerAddress { get; set; } = string.Empty;
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Assigned; // Assigned, PickedUp, Delivered, Failed

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; set; }
}

public enum DeliveryStatus
{
    Assigned = 1,
    PickedUp = 2,
    Delivered = 3,
    Failed = 4
}
