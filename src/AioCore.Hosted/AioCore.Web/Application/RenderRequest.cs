using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Scriban;
using Shared.Extensions;
using WebMarkupMin.Core;

namespace AioCore.Web.Application;

public class RenderRequest : IRequest<string>
{
    public string? Router { get; set; }

    public RenderRequest(string? router)
    {
        Router = router;
    }

    internal class Handler : IRequestHandler<RenderRequest, string>
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMemoryCache _memoryCache;

        public Handler(IWebHostEnvironment hostEnvironment, IMemoryCache memoryCache)
        {
            _hostEnvironment = hostEnvironment;
            _memoryCache = memoryCache;
        }

        public async Task<string> Handle(RenderRequest request, CancellationToken cancellationToken)
        {
            var pagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Pages", $"{request.Router}.liquid");
            var templateValue = _memoryCache.Get<string>(pagePath);
            if (string.IsNullOrEmpty(templateValue)) _memoryCache.Set(pagePath, pagePath.ReadFile());
            templateValue = _memoryCache.Get<string>(pagePath);
            var template = Template.Parse(templateValue);
            var output = await template.RenderAsync(new
            {
                Title = "AioCore",
                Heading = "Scriban",
                Products = new List<string>
                {
                    "xin chao",
                    "toi ten la Nguyen"
                }
            });
            return output;
        }
    }
}