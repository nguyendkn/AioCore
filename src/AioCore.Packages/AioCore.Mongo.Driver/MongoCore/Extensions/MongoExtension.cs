using System.Reflection;
using AioCore.Mongo.Driver.MongoCore.Abstracts;
using AioCore.Mongo.Driver.MongoCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver.MongoCore.Extensions;

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
}