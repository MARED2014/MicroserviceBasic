using PizzaPulse.Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Ordering.Core.Repositories;

// Interfaces/IOrderRepository.cs
public interface IOrderingRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order?> GetByIdWithItemsAsync(Guid id);
    Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);
    Task AddAsync(Order order);
    void Update(Order order);
    Task SaveChangesAsync();
}
