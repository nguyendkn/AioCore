using AioCore.Elasticsearch.Abstracts;
using AioCore.Elasticsearch.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace AioCore.Elasticsearch;

public static class Register
{
    public static void AddElasticsearchContext<TElasticsearchContext>(
        this IServiceCollection services, EsConfigs esConfigs)
        where TElasticsearchContext : EsContext
    {
        services.AddSingleton<IElasticClient>(_ =>
        {
            var node = new Uri(esConfigs.Url);
            var settings = new ConnectionSettings(node);
            if (string.IsNullOrEmpty(esConfigs.UserName)) return new ElasticClient(settings);
            settings.DefaultIndex(esConfigs.Index);
            settings.BasicAuthentication(esConfigs.UserName, esConfigs.Password);

            return new ElasticClient(settings);
        });
        services.AddSingleton<IEsContextBuilder>(provider =>
        {
            var requiredService = provider.GetRequiredService<IElasticClient>();
            return new EsContextBuilder(requiredService);
        });
        services.AddSingleton<TElasticsearchContext>();
    }
}