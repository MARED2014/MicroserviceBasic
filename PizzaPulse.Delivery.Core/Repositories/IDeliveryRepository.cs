using PizzaPulse.Delivery.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Delivery.Core.Repositories;

public interface IDeliveryRepository
{
    Task<DeliveryAssignment?> GetByOrderIdAsync(Guid orderId);
    Task AddAssignmentAsync(DeliveryAssignment assignment);
    Task SaveChangesAsync();
}
