using MongoDB.Driver;
using PizzaPulse.Kitchen.Core.Entities;
using PizzaPulse.Kitchen.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Kitchen.Infrastructure.Repositories;

public class KitchenTaskRepository : MongoRepository<KitchenTask>, IKitchenTaskRepository
{
    public KitchenTaskRepository(IMongoDatabase database) : base(database, "kitchen_tasks")
    {
    }

    public async Task<KitchenTask?> GetByOrderIdAsync(Guid orderId)
    {
        return await Collection.Find(x => x.OrderId == orderId).FirstOrDefaultAsync();
    }

    public async Task<List<KitchenTask>> GetPendingTasksAsync()
    {
        return await Collection.Find(x => x.Status != KitchenTaskStatus.Ready)
                               .SortBy(x => x.ReceivedAt)
                               .ToListAsync();
    }

    public Task UpdateByOrderIdAsync(KitchenTask task)
    {
        return Collection.ReplaceOneAsync(x => x.OrderId == task.OrderId, task);
    }
}
