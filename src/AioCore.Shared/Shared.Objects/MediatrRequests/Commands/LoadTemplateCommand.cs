using AioCore.Fluid.Core;
using Fluid;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Shared.Objects.MediatrRequests.Commands;

public class LoadTemplateCommand : IRequest<string>
{
    public string TemplateSlug { get; set; }

    public LoadTemplateCommand(string? templateSlug)
    {
        TemplateSlug = templateSlug ?? "/";
    }

    internal class Handler : IRequestHandler<LoadTemplateCommand, string>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly AioCoreContext _context;
        private readonly IFluidCoreService _fluidCoreService;

        public Handler(IMemoryCache memoryCache, AioCoreContext context,
            IFluidCoreService fluidCoreService)
        {
            _memoryCache = memoryCache;
            _context = context;
            _fluidCoreService = fluidCoreService;
        }

        public async Task<string> Handle(LoadTemplateCommand request, CancellationToken cancellationToken)
        {
            var templateHtml = _memoryCache.Get<string>(request.TemplateSlug);
            if (!string.IsNullOrEmpty(templateHtml))
                return await _fluidCoreService.RenderAsync(request.TemplateSlug, templateHtml);
            var templateEntity = await _context.Pages.FirstOrDefaultAsync(x => x.Slug.Equals(request.TemplateSlug));
            return await _fluidCoreService.RenderAsync(request.TemplateSlug, templateEntity?.TemplateUrl, true);
        }
    }
}