using Microsoft.EntityFrameworkCore;
using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;
using PizzaPulse.Delivery.Infrastructure.Contexts;

namespace PizzaPulse.Delivery.Infrastructure.Repositories;

public class CourierRepository : ICourierRepository
{
    private readonly DeliveryDbContext _context;

    public CourierRepository(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task<Courier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Couriers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Courier>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Couriers
            .OrderBy(c => c.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Courier>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Couriers
            .Where(c => c.IsActive)
            .OrderBy(c => c.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Courier courier, CancellationToken cancellationToken = default)
    {
        await _context.Couriers.AddAsync(courier, cancellationToken);
    }

    public void Update(Courier courier)
    {
        _context.Couriers.Update(courier);
    }

    public void Delete(Courier courier)
    {
        _context.Couriers.Remove(courier);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
