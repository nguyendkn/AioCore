using System.Linq.Expressions;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver.Abstracts;

public interface IMongoSet<TEntity>
{
    IMongoCollection<TEntity> Collection(string connectionString, string database);

    Task<TEntity> AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task<bool> UpdateAsync(object id, TEntity entity);

    Task<bool> RemoveAsync(object id);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

    Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CountOptions? options = null);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
}