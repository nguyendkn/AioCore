using Microsoft.Extensions.DependencyInjection;

namespace AioCore.Notion;

public static class NotionRegistration
{
    public static void AddAiocNotionClient(this IServiceCollection services)
    {
        var notionClient = new NotionClient();
        services.AddSingleton<INotionClient>(notionClient);
    }
}