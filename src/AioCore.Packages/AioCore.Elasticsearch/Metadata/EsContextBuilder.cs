using System.Reflection;
using AioCore.Elasticsearch.Abstracts;
using Humanizer;
using Nest;

namespace AioCore.Elasticsearch.Metadata;

public class EsContextBuilder : IEsContextBuilder
{
    public IElasticClient ElasticClient { get; }
    private bool IsConfigured { get; set; }

    public EsContextBuilder(IElasticClient elasticClient)
    {
        ElasticClient = elasticClient;
    }

    public void Entity<TEntity>(Action<EsTypeBuilder<TEntity>> action) where TEntity : class
    {
    }

    public void OnConfiguring(EsContext context)
    {
        if (IsConfigured) return;
        var contextProperties = context.GetType()
            .GetRuntimeProperties()
            .Where(
                p => !(p.GetMethod ?? p.SetMethod)!.IsStatic
                     && !p.GetIndexParameters().Any()
                     && p.DeclaringType != typeof(EsContext)
                     && p.PropertyType.GetTypeInfo().IsGenericType
                     && p.PropertyType.GetGenericTypeDefinition() == typeof(EsSet<>))
            .OrderBy(p => p.Name)
            .Select(
                p => (
                    p.Name,
                    Type: p.PropertyType.GenericTypeArguments.Single()
                ))
            .ToArray();

        foreach (var (name, type) in contextProperties)
        {
            var existed = ElasticClient.Indices.Exists(name);
            var esSet = typeof(EsSet<>).MakeGenericType(type);
            var dbSet = Activator.CreateInstance(esSet, ElasticClient);
            context.GetType().GetProperty(name)?.SetValue(context, dbSet);
        }

        IsConfigured = true;
    }

    public void OnModelCreating(Action action)
    {
        action.Invoke();
    }
}