using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace AioCore.Redis.OM;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedisContext<TRedisContext>(this IServiceCollection services,
        Action<RedisOptionsBuilder> action) where TRedisContext : RedisContext
    {
        services.Configure(action);
        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<RedisOptionsBuilder>>().Value;
            var lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect(options.ConnectionString));
            return lazyConnection;
        });

        services.AddSingleton<IRedisContextOptionsBuilder>(provider =>
        {
            var connection = provider.GetRequiredService<Lazy<ConnectionMultiplexer>>();
            return new RedisContextOptionsBuilder(connection);
        });
        
        services.AddSingleton<TRedisContext>();
        return services;
    }
}