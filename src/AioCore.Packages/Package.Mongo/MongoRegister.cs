using Microsoft.Extensions.DependencyInjection;

namespace Package.Mongo;

public static class MongoRegister
{
    public static void AddMongoContext<TMongoContext>(this IServiceCollection services)
        where TMongoContext : MongoContext
    {
        services.AddScoped<TMongoContext>();
    }
}