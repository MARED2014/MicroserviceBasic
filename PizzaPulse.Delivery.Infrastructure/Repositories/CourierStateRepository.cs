using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace PizzaPulse.Delivery.Infrastructure.Repositories;

public class CourierStateRepository : ICourierStateRepository
{
    private readonly IDatabase _database;

    public CourierStateRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task SetCourierStateAsync(ActiveCourierState state)
    {
        var key = $"courier:active:{state.CourierId}";
        await _database.StringSetAsync(key, JsonSerializer.Serialize(state));
    }

    public async Task<ActiveCourierState?> GetCourierStateAsync(Guid courierId)
    {
        var data = await _database.StringGetAsync($"courier:active:{courierId}");
        if (data.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<ActiveCourierState>((string)data);
    }

    // Basit mantık: Müsait (IsBusy = false) olan ilk kuryeyi bulur
    public async Task<Guid?> GetAvailableCourierIdAsync()
    {
        var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
        var keys = server.Keys(pattern: "courier:active:*");

        foreach (var key in keys)
        {
            var data = await _database.StringGetAsync(key);
            if (!data.IsNullOrEmpty)
            {
                var state = JsonSerializer.Deserialize<ActiveCourierState>((string)data);
                if (state != null && !state.IsBusy)
                {
                    return state.CourierId;
                }
            }
        }
        return null;
    }
}
