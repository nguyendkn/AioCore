using AioCore.Elasticsearch.Abstracts;
using Nest;

namespace AioCore.Elasticsearch;

public class EsContext
{
    protected readonly IEsContextBuilder ModelBuilder;
    public IElasticClient ElasticClient => ModelBuilder.ElasticClient;
    
    protected EsContext(IEsContextBuilder modelBuilder)
    {
        ModelBuilder = modelBuilder;
        modelBuilder.OnConfiguring(this);
        modelBuilder.OnModelCreating(OnModelCreating);
    }

    protected virtual void OnModelCreating()
    {
    }
}