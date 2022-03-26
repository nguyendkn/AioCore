using Microsoft.Extensions.Caching.Memory;
using Fluid;

namespace AioCore.Fluid.Core;

public interface IFluidCoreService
{
    string NotFoundTemplate();

    Task<FluidCoreRendered> RenderAsync(string slug, string? template, bool? online = false);
}

public class FluidCoreService : IFluidCoreService
{
    private readonly IMemoryCache _memoryCache;
    private readonly FluidCoreParser _fluidCoreParser;

    public FluidCoreService(IMemoryCache memoryCache, FluidCoreParser fluidCoreParser)
    {
        _memoryCache = memoryCache;
        _fluidCoreParser = fluidCoreParser;
    }

    public async Task<FluidCoreRendered> RenderAsync(string slug, string? template, bool? online = false)
    {
        if (string.IsNullOrEmpty(template))
            return new FluidCoreRendered {Rendered = NotFoundTemplate()};

        if (online == true)
        {
            var httpClient = new HttpClient();
            var templateHtml = await httpClient.GetStringAsync(template);
            _memoryCache.Set(slug, templateHtml);
            return await RenderFluid(templateHtml);
        }

        _memoryCache.Set(slug, template);
        return await RenderFluid(template);
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