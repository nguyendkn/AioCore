using System.Linq.Expressions;
using AioCore.Mongo.Abstracts;
using AioCore.Mongo.Metadata;
using AioCore.Shared.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace AioCore.Mongo;

public static class MongoExtension
{
    public static void AddMongoContext<TMongoContext>(
        this IServiceCollection services,
        MongoConfigs mongoConfigs)
        where TMongoContext : MongoContext
    {
        services.AddSingleton(_ =>
        {
            var settings = MongoClientSettings.FromConnectionString(mongoConfigs.ConnectionString);
            var client = new MongoClient(settings);

            return client.GetDatabase(mongoConfigs.Database);
        });
        services.AddSingleton<IMongoContextBuilder>(provider =>
        {
            var requiredService = provider.GetRequiredService<IMongoDatabase>();
            return new MongoContextBuilder(requiredService, mongoConfigs);
        });
        services.AddSingleton<TMongoContext>();
    }

    public static BsonDocument ToBsonQuery<T>(this FilterDefinition<T> filter)
    {
        var serializerRegistry = BsonSerializer.SerializerRegistry;
        var documentSerializer = serializerRegistry.GetSerializer<T>();
        return filter.Render(documentSerializer, serializerRegistry);
    }

    public static IAggregateFluent<BsonDocument>? Search(this IAggregateFluent<BsonDocument> aggregateFluent, int take)
    {
        return aggregateFluent.Limit(take);
    }

    public static IAggregateFluent<BsonDocument>? Take(this IAggregateFluent<BsonDocument> aggregateFluent, int take)
    {
        return aggregateFluent.Limit(take);
    }

    public static IFindFluent<TEntity, TEntity> Take<TEntity>(this IFindFluent<TEntity, TEntity> fluent, int? limit)
    {
        return fluent.Limit(limit);
    }

    public static IFindFluent<TEntity, TEntity> OrderBy<TEntity>(this IFindFluent<TEntity, TEntity> fluent,
        Expression<Func<TEntity, object>> expression)
    {
        return fluent.SortBy(expression);
    }

    public static IFindFluent<TEntity, TEntity> OrderByDescending<TEntity>(this IFindFluent<TEntity, TEntity> fluent,
        Expression<Func<TEntity, object>> expression)
    {
        return fluent.SortByDescending(expression);
    }

    public static async Task<bool> UpdateAsync<TEntity>(
        this IMongoCollection<TEntity> collection, Guid id, TEntity entity)
        where TEntity : MongoDocument
    {
        var replaceOneResult = await collection.ReplaceOneAsync(x => x!.Id.Equals(id), entity);
        return replaceOneResult.IsAcknowledged;
    }

    public static async Task<TEntity?> FirstOrDefaultAsync<TEntity>(
        this IMongoCollection<TEntity> collection,
        Expression<Func<TEntity, bool>> expression)
        where TEntity : MongoDocument
    {
        var document = await collection.FindAsync(expression);
        return await document.FirstOrDefaultAsync();
    }

    public static async Task<TEntity> AddAsync<TEntity>(
        this IMongoCollection<TEntity> collection, TEntity entity)
        where TEntity : MongoDocument
    {
        await collection.InsertOneAsync(entity);
        return entity;
    }

    public static async Task AddRangeAsync<TEntity>(
        this IMongoCollection<TEntity> collection,
        IEnumerable<TEntity> entities)
        where TEntity : MongoDocument
    {
        await collection.InsertManyAsync(entities);
    }
}