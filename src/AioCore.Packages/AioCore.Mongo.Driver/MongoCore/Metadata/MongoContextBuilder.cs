using System.Reflection;
using AioCore.Mongo.Driver.MongoCore.Abstracts;
using MongoDB.Driver;

namespace AioCore.Mongo.Driver.MongoCore.Metadata;

public class MongoContextBuilder : IMongoContextBuilder
{
    public IMongoDatabase Database { get; }
    private bool IsConfigured { get; set; }

    public MongoContextBuilder(IMongoDatabase database)
    {
        Database = database;
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

    public void OnModelCreating()
    {
    }
}