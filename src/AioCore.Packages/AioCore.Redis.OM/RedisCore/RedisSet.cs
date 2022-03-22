using System.Collections;
using System.Linq.Expressions;
using AioCore.Redis.OM.Searching;

namespace AioCore.Redis.OM.RedisCore;

public abstract class RedisSet<TEntity> : IQueryable<TEntity>
{
    protected RedisSet(IRedisCollection<TEntity> collection, RedisContext context)
    {
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        => throw new NotSupportedException();

    IEnumerator IEnumerable.GetEnumerator()
        => throw new NotSupportedException();

    Type IQueryable.ElementType
        => throw new NotSupportedException();

    Expression IQueryable.Expression
        => throw new NotSupportedException();

    IQueryProvider IQueryable.Provider
        => throw new NotSupportedException();
}