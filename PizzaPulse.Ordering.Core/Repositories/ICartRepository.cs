using PizzaPulse.Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Ordering.Core.Repositories;

public interface ICartRepository
{
    Task<List<CartItem>> GetCartAsync(string customerId);
    Task AddOrUpdateCartItemAsync(string customerId, CartItem item);
    Task ClearCartAsync(string customerId);
}
