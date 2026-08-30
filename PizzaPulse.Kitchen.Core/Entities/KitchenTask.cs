using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Kitchen.Core.Entities;

public class KitchenTask
{
    //[BsonId]
    //[BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public Guid OrderId { get; set; } // Order Service'ten gelen referans ID
    public string DeliveryAddress { get; set; } = string.Empty;
    public List<string> ItemsSummary { get; set; } = new(); // Örn: ["1x Large Pepperoni", "2x Medium Margherita"]

    public KitchenTaskStatus Status { get; set; } = KitchenTaskStatus.Waiting; // Waiting, InOven, Ready
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public DateTime? OvenStartedAt { get; set; }
    public DateTime? BakedAt { get; set; }
}

public enum KitchenTaskStatus
{
    Waiting = 1,
    InOven = 2,
    Ready = 3
}
