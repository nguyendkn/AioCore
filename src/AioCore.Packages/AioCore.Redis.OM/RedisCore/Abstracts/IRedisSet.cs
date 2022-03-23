using System.Linq.Expressions;

namespace AioCore.Redis.OM.RedisCore.Abstracts;

public interface IRedisSet<TEntity>
{
    Task<TEntity> FindAsync(object key);

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

    Task<TEntity> AddAsync(TEntity entity);
}