using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Shared.Objects.MediatrRequests.Commands;

public class LoadTemplateCommand : IRequest<string>
{
    public string TemplateUrl { get; set; }

    public LoadTemplateCommand(string? templateUrl)
    {
        TemplateUrl = templateUrl ?? "/";
    }

    internal class Handler : IRequestHandler<LoadTemplateCommand, string>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly AioCoreContext _context;

        public Handler(IMemoryCache memoryCache, AioCoreContext context)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

        public async Task<string> Handle(LoadTemplateCommand request, CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();

            var template = _memoryCache.Get<string>(request.TemplateUrl);
            if (!string.IsNullOrEmpty(template)) return template;

            var templateEntity = await _context.Pages.FirstOrDefaultAsync(x => x.Slug.Equals(request.TemplateUrl));
            template = await httpClient.GetStringAsync(templateEntity.Template, cancellationToken);
            _memoryCache.Set(request.TemplateUrl, template);

            return template;
        }
    }
}