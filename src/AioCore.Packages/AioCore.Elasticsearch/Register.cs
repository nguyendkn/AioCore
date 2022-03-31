using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace AioCore.Elasticsearch;

public static class Register
{
    public static void AddElasticsearchContext<TElasticsearchContext>(
        this IServiceCollection services, string connectionString)
        where TElasticsearchContext : EsContext
    {
        services.AddSingleton<IElasticClient>(_ =>
        {
            var node = new Uri(connectionString);
            var settings = new ConnectionSettings(node);
            return new ElasticClient(settings);
        });
    }
}