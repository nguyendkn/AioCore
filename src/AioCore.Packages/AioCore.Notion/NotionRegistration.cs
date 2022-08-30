using Microsoft.Extensions.DependencyInjection;

namespace AioCore.Notion;

public static class NotionRegistration
{
    public static void AddAiocNotionClient(this IServiceCollection services, NotionOptions notionOptions)
    {
        var notionClient = new NotionClient(notionOptions);
        services.AddSingleton(notionOptions);
        services.AddSingleton<INotionClient>(notionClient);
    }
}