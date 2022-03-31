using System.Reflection;
using AioCore.Mongo.Driver.Abstracts;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver.Metadata;

public class MongoContextBuilder : IMongoContextBuilder
{
    public IMongoDatabase Database { get; }
    private bool IsConfigured { get; set; }

    private readonly IDictionary<Type, object> _entityToBuilderMap =
        new Dictionary<Type, object>();

    public MongoContextBuilder(IMongoDatabase database)
    {
        Database = database;
    }

    public void Entity<TEntity>(Action<EntityTypeBuilder<TEntity>> action) where TEntity : class
    {
        var builder = _entityToBuilderMap.ContainsKey(typeof(TEntity))
            ? _entityToBuilderMap[typeof(TEntity)] as EntityTypeBuilder<TEntity>
            : new EntityTypeBuilder<TEntity>(Database);
        action.Invoke(builder ?? throw new ArgumentException(typeof(TEntity).Name));
        _entityToBuilderMap[typeof(TEntity)] = builder;
    }

    public void OnConfiguring(MongoContext context)
    {
        if (IsConfigured) return;
        var contextProperties = context.GetType()
            .GetRuntimeProperties()
            .Where(
                p => !(p.GetMethod ?? p.SetMethod)!.IsStatic
                     && !p.GetIndexParameters().Any()
                     && p.DeclaringType != typeof(MongoContext)
                     && p.PropertyType.GetTypeInfo().IsGenericType
                     && p.PropertyType.GetGenericTypeDefinition() == typeof(MongoSet<>))
            .OrderBy(p => p.Name)
            .Select(
                p => (
                    p.Name,
                    Type: p.PropertyType.GenericTypeArguments.Single()
                ))
            .ToArray();

        foreach (var (name, type) in contextProperties)
        {
            var mongoSet = typeof(MongoSet<>).MakeGenericType(type);
            var dbSet = Activator.CreateInstance(mongoSet, Database);
            context.GetType().GetProperty(name)?.SetValue(context, dbSet);
        }

        IsConfigured = true;
    }

    public void OnModelCreating(Action action)
    {
        action.Invoke();
    }

    private void InvokeEntityBuildersMethod(string methodName)
    {
        foreach (var (key, _) in _entityToBuilderMap)
        {
            var builderType = typeof(EntityTypeBuilder<>).MakeGenericType(key);
            var builder = _entityToBuilderMap[key];

            builderType.InvokeMember(methodName,
                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                Type.DefaultBinder, builder, null);
        }
    }
}