using System;
using AioCore.Elasticsearch.Abstracts;

namespace AioCore.Elasticsearch.Tests;

public class AioCoreContext : EsContext, IDisposable
{
    public AioCoreContext(IEsContextBuilder modelBuilder) : base(modelBuilder)
    {
    }

    public EsSet<User> Users { get; set; } = default!;
    
    public void Dispose()
    {
    }
}