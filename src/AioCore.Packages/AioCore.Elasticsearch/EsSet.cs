using System.Collections;
using System.Linq.Expressions;
using AioCore.Elasticsearch.Abstracts;
using Nest;

namespace AioCore.Elasticsearch;

public class EsSet<TEntity> : IQueryable<TEntity>, IEsSet<TEntity>
    where TEntity : EsDocument
{
    private readonly IElasticClient _elasticClient;

    public EsSet(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public IEnumerator<TEntity> GetEnumerator()
        => throw new NotSupportedException();

    IEnumerator IEnumerable.GetEnumerator()
        => throw new NotSupportedException();

    public Type ElementType
        => throw new NotSupportedException();

    public Expression Expression
        => throw new NotSupportedException();

    public IQueryProvider Provider
        => throw new NotSupportedException();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var response = await _elasticClient.IndexAsync(entity, idx => idx.Index<TEntity>());
        return default!;
    }

    public Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(object id, TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(object id)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<long> CountAsync(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }
}