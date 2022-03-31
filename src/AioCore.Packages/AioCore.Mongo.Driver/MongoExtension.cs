using System.Linq.Expressions;
using AioCore.Mongo.Driver.Abstracts;
using AioCore.Mongo.Driver.Metadata;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver;

public static class MongoExtension
{
    public static void AddMongoContext<TMongoContext>(
        this IServiceCollection services,
        string connectionString, string database)
        where TMongoContext : MongoContext
    {
        services.AddSingleton(_ =>
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);

            return client.GetDatabase(database);
        });
        services.AddSingleton<IMongoContextBuilder>(provider =>
        {
            var requiredService = provider.GetRequiredService<IMongoDatabase>();
            return new MongoContextBuilder(requiredService);
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
}