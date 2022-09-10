using AioCore.Services;

namespace AioCore.Web.MiddleWares;

public class StaticRenderMiddleWare
{
    private readonly RequestDelegate _next;

    public StaticRenderMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context,
        IClientService clientService, ITemplateService templateService)
    {
        await _next(context);
        await BuildStaticRender(context,
            clientService, templateService);
    }

    private static async Task BuildStaticRender(
        HttpContext context,
        IClientService clientService,
        ITemplateService templateService)
    {
        if (IgnoreCase.Any(x => clientService.RequestUrl().ToLower().StartsWith(x))) return;
        var host = clientService.RequestUrl();
        var template = await templateService.Render(host);
        await context.Response.WriteAsync(template);
    }

    private static List<string> IgnoreCase => new()
    {
        "/_blazor",
        "/identity",
        "/static"
    };
}

public static class StaticRenderMiddleWareExtension
{
    public static void UseStaticRender(this WebApplication application)
    {
        application.UseMiddleware<StaticRenderMiddleWare>();
    }
}