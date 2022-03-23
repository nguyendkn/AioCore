using System.Collections;
using System.Linq.Expressions;
using AioCore.Redis.OM.Aggregation;
using AioCore.Redis.OM.RedisCore.Abstracts;
using AioCore.Redis.OM.Searching;

namespace AioCore.Redis.OM.RedisCore;

public class RedisSet<TEntity> : IQueryable<TEntity>, IRedisSet<TEntity> where TEntity : notnull
{
    private readonly IRedisCollection<TEntity> _collection;
    private readonly RedisAggregationSet<TEntity> _aggregationSet;

    public RedisSet(RedisConnectionProvider provider)
    {
        _collection = provider.RedisCollection<TEntity>();
        _aggregationSet = provider.AggregationSet<TEntity>();
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        => _collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _collection.GetEnumerator();

    Type IQueryable.ElementType
        => _collection.ElementType;

    Expression IQueryable.Expression
        => _collection.Expression;

    IQueryProvider IQueryable.Provider
        => _collection.Provider;

    public async Task<TEntity> FindAsync(object key)
    {
        var keyString = $"{typeof(TEntity).FullName}:{key}";
        return await _collection.FindByIdAsync(keyString);
    }

    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _collection.FirstOrDefaultAsync(expression);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity = await _collection.InsertAsync(entity);
        return entity;
    }
}