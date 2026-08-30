using Microsoft.EntityFrameworkCore;
using PizzaPulse.Ordering.Core.Entities;
using PizzaPulse.Ordering.Core.Repositories;
using PizzaPulse.Ordering.Infrastructure.Contexts;

namespace PizzaPulse.Ordering.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly OrderDbContext _context;

    public MenuRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<PizzaMenu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PizzaMenus
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PizzaMenu>> GetAllAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PizzaMenus
            .AsNoTracking()
            .Where(p => p.IsAvailable)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PizzaMenu pizzaMenu, CancellationToken cancellationToken = default)
    {
        await _context.PizzaMenus.AddAsync(pizzaMenu, cancellationToken);
    }

    public void Update(PizzaMenu pizzaMenu)
    {
        _context.PizzaMenus.Update(pizzaMenu);
    }

    public void Delete(PizzaMenu pizzaMenu)
    {
        _context.PizzaMenus.Remove(pizzaMenu);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
