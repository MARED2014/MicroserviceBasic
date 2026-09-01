using PizzaPulse.Delivery.Core.Entities;

namespace PizzaPulse.Delivery.Core.Repositories;

public interface ICourierRepository
{
    Task<Courier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Courier>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Courier>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Courier courier, CancellationToken cancellationToken = default);
    void Update(Courier courier);
    void Delete(Courier courier);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
