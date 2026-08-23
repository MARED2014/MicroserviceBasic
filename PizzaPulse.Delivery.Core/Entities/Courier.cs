using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Delivery.Core.Entities;

public class Courier
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
