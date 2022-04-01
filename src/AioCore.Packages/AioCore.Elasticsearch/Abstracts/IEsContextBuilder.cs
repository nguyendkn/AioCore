using AioCore.Elasticsearch.Metadata;
using Nest;

namespace AioCore.Elasticsearch.Abstracts;

public interface IEsContextBuilder
{
    IElasticClient ElasticClient { get; }

    void Entity<TEntity>(Action<EsTypeBuilder<TEntity>> action) where TEntity : class;

    void OnConfiguring(EsContext context);

    void OnModelCreating(Action action);
}