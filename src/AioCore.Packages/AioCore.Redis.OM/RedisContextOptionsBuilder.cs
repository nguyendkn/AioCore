using System.Reflection;
using StackExchange.Redis;

namespace AioCore.Redis.OM;

public class RedisContextOptionsBuilder : IRedisContextOptionsBuilder
{
    public Lazy<ConnectionMultiplexer> LazyConnection { get; }

    public RedisContextOptionsBuilder(Lazy<ConnectionMultiplexer> lazyConnection)
    {
        LazyConnection = lazyConnection;
    }

    public void Configure(RedisContext context, Action configAction)
    {
        configAction.Invoke();
        var contextProperties = context.GetType().GetRuntimeProperties()
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

        var index = 1;
        var database = new List<RedisDatabase>();
        foreach (var (name, type) in contextProperties)
        {
            database.Add(new RedisDatabase(type, index));
            var dbSetType = typeof(RedisSet<>).MakeGenericType(type);
            var dbSet = Activator.CreateInstance(dbSetType, this, context, LazyConnection);
            context.GetType().GetProperty(name)?.SetValue(context, dbSet);
            index++;
        }

        RedisContext.Database = database;
    }
}