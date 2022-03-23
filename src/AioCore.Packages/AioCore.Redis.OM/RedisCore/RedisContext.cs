using System.Reflection;

namespace AioCore.Redis.OM.RedisCore;

public class RedisContext
{
    protected RedisContext(RedisConnectionProvider provider)
    {
        Configure(this, provider);
    }

    private static void Configure(RedisContext context, RedisConnectionProvider provider)
    {
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

        foreach (var (name, type) in contextProperties)
        {
            var dbSetType = typeof(RedisSet<>).MakeGenericType(type);
            var dbSet = Activator.CreateInstance(dbSetType, provider);

            context.GetType().GetProperty(name)?.SetValue(context, dbSet);
        }
    }
}