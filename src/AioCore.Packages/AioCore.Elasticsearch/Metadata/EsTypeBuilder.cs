using Nest;

namespace AioCore.Elasticsearch.Metadata;

public class EsTypeBuilder<TEntity>
{
    private readonly IElasticClient _elasticClient;

    public EsTypeBuilder(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }
}