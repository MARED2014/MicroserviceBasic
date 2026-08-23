using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPulse.Kitchen.Core.Repositories;

public interface IMongoRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task CreateAsync(T entity);
    Task UpdateAsync(Guid id, T entity);
}
