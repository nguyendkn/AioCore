namespace AioCore.Mongo.OM.MongoCore.Abstracts;

public interface IMongoSet<TEntity>
{
    Task<TEntity> AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task<bool> UpdateAsync(object id, TEntity entity);

    Task<bool> RemoveAsync(object id);
}