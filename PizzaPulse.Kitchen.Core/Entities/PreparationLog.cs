using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Kitchen.Core.Entities;

// Analiz ve telemetri amaçlı saklanan pişirme performans günlüğü
public class PreparationLog
{
    //[BsonId]
    //[BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public double TotalPreparationTimeSeconds { get; set; } // Sipariş gelişinden fırından çıkışa kadar geçen süre
    public double OvenTimeSeconds { get; set; } // Fırında kaldığı net süre
    public string ChefNotes { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}
