using System.Net.Http.Headers;
using Fluid;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace AioCore.Fluid.Core;

public interface IFluidCoreService
{
    string NotFoundTemplate();

    Task<FluidCoreRendered> RenderAsync(string slug, string? template, bool online = false);
}

public class FluidCoreService : IFluidCoreService
{
    private readonly IMemoryCache _memoryCache;
    private readonly FluidCoreParser _fluidCoreParser;
    private readonly IWebHostEnvironment _environment;

    public FluidCoreService(IMemoryCache memoryCache, FluidCoreParser fluidCoreParser, IWebHostEnvironment environment)
    {
        _memoryCache = memoryCache;
        _fluidCoreParser = fluidCoreParser;
        _environment = environment;
    }

    public async Task<FluidCoreRendered> RenderAsync(string slug, string? template, bool online = false)
    {
        if (string.IsNullOrEmpty(template))
            return new FluidCoreRendered {Rendered = NotFoundTemplate()};

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {NoCache = _environment.IsDevelopment()};

        var templateHtml = _memoryCache.Get<string>(slug);
        if (templateHtml != null)
            return await RenderFluid(template);

        templateHtml = online ? await httpClient.GetStringAsync(template) : template;
        _memoryCache.Set(slug, templateHtml);
        return await RenderFluid(templateHtml);
    }

    private async Task<FluidCoreRendered> RenderFluid(string? fluidTemplate)
    {
        var result = _fluidCoreParser.TryParse(fluidTemplate, out var template, out var error);
        return new FluidCoreRendered
        {
            Rendered = result ? await template.RenderAsync() : NotFoundTemplate(),
            Error = error
        };
    }

    public string NotFoundTemplate() => "<html></html>";
}