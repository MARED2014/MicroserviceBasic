using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PizzaPulse.Ordering.Infrastructure.Repositories;

// Repositories/CartRepository.cs
public class CartRepository : ICartRepository
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public CartRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public async Task<List<CartItem>> GetCartAsync(string customerId)
    {
        var data = await _database.StringGetAsync($"cart:{customerId}");
        if (data.IsNullOrEmpty) return new List<CartItem>();

        return JsonSerializer.Deserialize<List<CartItem>>((string)data) ?? new List<CartItem>();
    }

    public async Task AddOrUpdateCartItemAsync(string customerId, CartItem item)
    {
        var cart = await GetCartAsync(customerId);
        var existing = cart.FirstOrDefault(c => c.PizzaMenuId == item.PizzaMenuId && c.Size == item.Size);

        if (existing != null)
        {
            existing.Quantity += item.Quantity;
        }
        else
        {
            cart.Add(item);
        }

        await _database.StringSetAsync($"cart:{customerId}", JsonSerializer.Serialize(cart), TimeSpan.FromDays(7));
    }

    public async Task ClearCartAsync(string customerId)
    {
        await _database.KeyDeleteAsync($"cart:{customerId}");
    }
}
