using PizzaPulse.Ordering.Core.Entities;

namespace PizzaPulse.Ordering.Core.Repositories;

public interface IMenuRepository
{
    Task<PizzaMenu?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PizzaMenu>> GetAllAvailableAsync(CancellationToken cancellationToken = default);
    Task AddAsync(PizzaMenu pizzaMenu, CancellationToken cancellationToken = default);
    void Update(PizzaMenu pizzaMenu);
    void Delete(PizzaMenu pizzaMenu);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}