using PizzaPulse.Delivery.Core.Entities;
using PizzaPulse.Delivery.Core.Repositories;
using PizzaPulse.Delivery.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace PizzaPulse.Delivery.Infrastructure.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly DeliveryDbContext _context;

    public DeliveryRepository(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task<DeliveryAssignment?> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.DeliveryAssignments.FirstOrDefaultAsync(d => d.OrderId == orderId);
    }

    public async Task<IEnumerable<Courier>> GetActiveCouriersAsync()
    {
        return await _context.Couriers
            .Where(c => c.IsActive)
            .ToListAsync();
    }

    public async Task AddAssignmentAsync(DeliveryAssignment assignment)
    {
        await _context.DeliveryAssignments.AddAsync(assignment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
