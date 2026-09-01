using MongoDB.Driver;
using PizzaPulse.Kitchen.Core.Repositories;

namespace PizzaPulse.Kitchen.Infrastructure.Repositories;

public class MongoRepository<T> : IMongoRepository<T> where T : class
{
    protected readonly IMongoCollection<T> Collection;

    public MongoRepository(IMongoDatabase database)
        : this(database, typeof(T).Name.ToLowerInvariant() + "s")
    {
    }

    protected MongoRepository(IMongoDatabase database, string collectionName)
    {
        Collection = database.GetCollection<T>(collectionName);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await Collection.Find(_ => true).ToListAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(Guid id, T entity)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        await Collection.ReplaceOneAsync(filter, entity);
    }
}
