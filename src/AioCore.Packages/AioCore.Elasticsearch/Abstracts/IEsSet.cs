using System.Linq.Expressions;

namespace AioCore.Elasticsearch.Abstracts;

public interface IEsSet<TEntity>
{
    Task<TEntity> AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task<bool> UpdateAsync(object id, TEntity entity);

    Task<bool> RemoveAsync(object id);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

    Task<long> CountAsync(Expression<Func<TEntity, bool>> expression);
}