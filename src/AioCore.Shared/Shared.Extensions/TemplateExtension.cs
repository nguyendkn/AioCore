using Microsoft.Extensions.Caching.Memory;

namespace Shared.Extensions;

public static class TemplateExtension
{
    public static async Task<string> LoadTemplate(this IMemoryCache memoryCache, string templateUrl)
    {
        var httpClient = new HttpClient();
        var template = memoryCache.Get<string>(templateUrl);
        if (!string.IsNullOrEmpty(template)) return template;

        template = await httpClient.GetStringAsync(templateUrl);
        memoryCache.Set(templateUrl, template);

        return template;
    }
}