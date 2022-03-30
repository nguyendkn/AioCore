using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver.MongoCore.Extensions;

public static class QueryExtension
{
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