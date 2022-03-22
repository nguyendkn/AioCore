using System.Reflection;

namespace AioCore.Redis.OM.RedisCore;

public class RedisContext
{
    public RedisContext(RedisConnectionProvider provider)
    {
        Configure(this);
    }

    private void Configure(RedisContext context)
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
            var dbSet = Activator.CreateInstance(dbSetType, this, context);

            context.GetType().GetProperty(name)?.SetValue(context, dbSet);
        }
    }
}