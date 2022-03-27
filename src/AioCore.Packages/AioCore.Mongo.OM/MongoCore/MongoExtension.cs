using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AioCore.Mongo.OM.MongoCore;

public static class MongoExtension
{
    public static void AddMongoContext<TMongoContext>(this IServiceCollection services,
        string connectionString, string database) where TMongoContext : MongoContext, new()
    {
        var mongoContext = new TMongoContext();
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(database);
        var contextProperties = mongoContext.GetType()
            .GetRuntimeProperties()
            .Where(
                p => !(p.GetMethod ?? p.SetMethod)!.IsStatic
                     && !p.GetIndexParameters().Any()
                     && p.DeclaringType != typeof(MongoContext)
                     && p.PropertyType.GetTypeInfo().IsGenericType
                     && p.PropertyType.GetGenericTypeDefinition() == typeof(MongoSet<>))
            .OrderBy(p => p.Name)
            .Select(
                p => (
                    p.Name,
                    Type: p.PropertyType.GenericTypeArguments.Single()
                ))
            .ToArray();

        foreach (var (name, type) in contextProperties)
        {
            var mongoSet = typeof(MongoSet<>).MakeGenericType(type);
            var dbSet = Activator.CreateInstance(mongoSet, mongoDatabase);
            mongoContext.GetType().GetProperty(name)?.SetValue(mongoContext, dbSet);
        }

        services.AddSingleton(mongoClient);
        services.AddSingleton(mongoContext);
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