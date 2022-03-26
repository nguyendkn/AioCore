using AioCore.Fluid.Core;
using Fluid;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Shared.Objects.MediatrRequests.Commands;

public class LoadTemplateCommand : IRequest<string>
{
    public string? TemplateUrl { get; set; }

    public LoadTemplateCommand(string? templateUrl)
    {
        TemplateUrl = templateUrl ?? "/";
    }

    internal class Handler : IRequestHandler<LoadTemplateCommand, string>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly AioCoreContext _context;
        private readonly FluidCoreParser _fluidCoreParser;

        public Handler(IMemoryCache memoryCache, AioCoreContext context,
            FluidCoreParser fluidCoreParser)
        {
            _memoryCache = memoryCache;
            _context = context;
            _fluidCoreParser = fluidCoreParser;
        }

        public async Task<string> Handle(LoadTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = _memoryCache.Get<string>(request.TemplateUrl);
            if (!string.IsNullOrEmpty(template)) return await RenderFluid(template);

            var templateEntity = await _context.Pages.FirstOrDefaultAsync(x => x.Slug.Equals(request.TemplateUrl));
            if (string.IsNullOrEmpty(templateEntity?.Template)) return NotFoundTemplate;

            var httpClient = new HttpClient();
            template = await httpClient.GetStringAsync(templateEntity.Template, cancellationToken);
            _memoryCache.Set(request.TemplateUrl, template);

            return await RenderFluid(template);
        }

        private async Task<string> RenderFluid(string fluidTemplate)
        {
            var result = _fluidCoreParser.TryParse(fluidTemplate, out var template, out var error);
            return result ? await template.RenderAsync() : NotFoundTemplate;
        }

        private static string NotFoundTemplate => "<html></html>";
    }
}