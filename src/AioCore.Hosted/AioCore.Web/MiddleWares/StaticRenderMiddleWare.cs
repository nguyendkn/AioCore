using System.Net;
using System.Text;
using AioCore.Services;
using AioCore.Shared.Common.Constants;

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
        if (!SystemFeatures.Authorized.Any(x => clientService.RequestUrl().ToLower().StartsWith(x)) &&
            !SystemFeatures.Anonymous.Any(x => clientService.RequestUrl().ToLower().StartsWith(x)))
            await RenderStaticPage(context, clientService, templateService);
        else await _next(context);
    }

    private static async Task RenderStaticPage(
        HttpContext context,
        IClientService clientService,
        ITemplateService templateService)
    {
        if (SystemFeatures.Authorized.Any(x => clientService.RequestUrl().ToLower().StartsWith(x))) return;
        var host = clientService.RequestUrl();
        var userAgent = context.Request.Headers.UserAgent.ToString();
        var template = await templateService.Render(host, userAgent);
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = "text/html";
        context.Response.ContentLength = template.Length;
        await using var stream = context.Response.Body;
        await stream.WriteAsync(Encoding.UTF8.GetBytes(template).AsMemory(0, template.Length));
        await stream.FlushAsync();
    }
}

public static class StaticRenderMiddleWareExtension
{
    public static void UseStaticRender(this WebApplication application)
    {
        application.UseMiddleware<StaticRenderMiddleWare>();
    }
}