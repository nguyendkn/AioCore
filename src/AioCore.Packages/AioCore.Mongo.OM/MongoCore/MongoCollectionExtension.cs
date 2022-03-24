using MongoDB.Driver;

namespace AioCore.Mongo.OM.MongoCore;

public static class MongoCollectionExtension
{
    public static IFindFluent<TEntity, TEntity> Take<TEntity>(this IFindFluent<TEntity, TEntity> fluent, int? limit)
    {
        return fluent.Limit(limit);
    }
}