using PizzaPulse.Delivery.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Delivery.Core.Repositories;

public interface ICourierStateRepository
{
    Task SetCourierStateAsync(ActiveCourierState state);
    Task<ActiveCourierState?> GetCourierStateAsync(Guid courierId);
    Task<Guid?> GetAvailableCourierIdAsync();
}
