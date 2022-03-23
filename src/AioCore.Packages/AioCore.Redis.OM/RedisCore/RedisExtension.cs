using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AioCore.Redis.OM.RedisCore;

public static class RedisExtension
{
    public static void AddRedisContext<TRedisContext>(this IServiceCollection services,
        string connectionString) where TRedisContext : RedisContext, new()
    {
        var redisProvider = new RedisConnectionProvider(connectionString);
        var redisContext = new TRedisContext();
        var indexes = redisProvider.Connection.Execute("FT._LIST").ToArray()
            .Select(x => x.ToString(CultureInfo.CurrentCulture)).ToList();

        var contextProperties = typeof(TRedisContext).GetRuntimeProperties()
            .Where(
                p => !(p.GetMethod ?? p.SetMethod)!.IsStatic
                     && !p.GetIndexParameters().Any()
                     && p.DeclaringType != typeof(RedisContext)
                     && p.PropertyType.GetTypeInfo().IsGenericType
                     && p.PropertyType.GetGenericTypeDefinition() == typeof(RedisSet<>))
            .OrderBy(p => p.Name)
            .Select(
                p => (
                    p.Name,
                    Type: p.PropertyType.GenericTypeArguments.Single()
                ))
            .ToArray();
        
        foreach (var (name, type) in contextProperties)
        {
            var dbSetType = typeof(RedisSet<>).MakeGenericType(type);
            var dbSet = Activator.CreateInstance(dbSetType, redisProvider);

            redisContext.GetType().GetProperty(name)?.SetValue(redisContext, dbSet);
            if (indexes.All(x => x != $"{type.Name.ToLower()}-idx"))
                redisProvider.Connection.CreateIndex(type);
        }

        services.AddSingleton(redisContext);
    }
}