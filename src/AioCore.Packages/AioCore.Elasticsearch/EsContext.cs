using AioCore.Elasticsearch.Abstracts;

namespace AioCore.Elasticsearch;

public class EsContext
{
    protected readonly IEsContextBuilder ModelBuilder;
    
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