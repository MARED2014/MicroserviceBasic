using PizzaPulse.Kitchen.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Kitchen.Core.Repositories;

public interface IKitchenTaskRepository : IMongoRepository<KitchenTask>
{
    Task<KitchenTask?> GetByOrderIdAsync(Guid orderId);
    Task<List<KitchenTask>> GetPendingTasksAsync();
    Task UpdateByOrderIdAsync(KitchenTask task);
}
