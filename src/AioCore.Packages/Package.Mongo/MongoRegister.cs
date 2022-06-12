using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Package.Mongo;

public static class MongoRegister
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
}