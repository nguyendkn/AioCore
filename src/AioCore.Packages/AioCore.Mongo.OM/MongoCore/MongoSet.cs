using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using AioCore.Mongo.OM.Attributes;
using AioCore.Mongo.OM.MongoCore.Abstracts;
using Humanizer;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AioCore.Mongo.OM.MongoCore;

public class MongoSet<TEntity> : IQueryable<TEntity>, IMongoSet<TEntity>
    where TEntity : MongoDocument
{
    private readonly IMongoCollection<TEntity> _collection;

    public MongoSet(IMongoDatabase database)
    {
        var connectionName = typeof(TEntity).Name.Pluralize();
        _collection = database.GetCollection<TEntity>(connectionName);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _collection.InsertManyAsync(entities);
    }

    public async Task<bool> UpdateAsync(object id, TEntity entity)
    {
        var record = await _collection.ReplaceOneAsync(x => x!.Id.Equals(id), entity);
        return record.IsAcknowledged;
    }

    public async Task<bool> RemoveAsync(object id)
    {
        var deleteResult = await _collection.DeleteOneAsync(x => x!.Id.Equals(id));
        return deleteResult.IsAcknowledged;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
    {
        var document = await _collection.FindAsync(expression);
        return await document.FirstOrDefaultAsync();
    }

    public async Task<TEntity> FindAsync(object key)
    {
        if (key.Equals(Guid.Empty)) return default!;
        var document = await _collection.FindAsync(x => x!.Id.Equals(key));
        return await document.FirstOrDefaultAsync();
    }

    public IFindFluent<TEntity, TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        var filter = Builders<TEntity>.Filter.Where(expression);
        var fluent = _collection.Find(filter);
        return fluent;
    }

    public IAggregateFluent<BsonDocument> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        var targetProperty = typeof(TProperty).Name;
        var targetCollection = targetProperty.Pluralize();
        var localFieldProperty = typeof(TEntity).GetProperties()
            .FirstOrDefault(x =>
                x.GetCustomAttribute<MongoLocalFieldAttribute>() != null);
        var localField = localFieldProperty?.GetCustomAttribute<MongoLocalFieldAttribute>()?.LocalField;
        var foreignField = typeof(TProperty).GetProperties()
            .FirstOrDefault(x =>
                x.GetCustomAttribute<MongoKeyAttribute>() != null)?.Name;
        return _collection.Aggregate().Lookup(targetCollection, localField, foreignField, localFieldProperty?.Name);
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        => _collection.AsQueryable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _collection.AsQueryable().GetEnumerator();

    Type IQueryable.ElementType
        => _collection.AsQueryable().ElementType;

    Expression IQueryable.Expression
        => _collection.AsQueryable().Expression;

    IQueryProvider IQueryable.Provider
        => _collection.AsQueryable().Provider;
}