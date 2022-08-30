using AioCore.Notion;
using AioCore.Shared.SeedWorks;
using MediatR;

namespace AioCore.Read.DynamicQueries;

public class GetNotionPageQuery : IRequest<Response<string>>
{
    public string Page { get; set; }

    public GetNotionPageQuery(string page)
    {
        Page = page;
    }

    internal class Handler : IRequestHandler<GetNotionPageQuery, Response<string>>
    {
        private readonly INotionClient _notionClient;

        public Handler(INotionClient notionClient)
        {
            _notionClient = notionClient;
        }

        public async Task<Response<string>> Handle(GetNotionPageQuery request, CancellationToken cancellationToken)
        {
            var page = await _notionClient.GetPageAsync(request.Page);
            var html = page.ToHtml();
            return new Response<string>
            {
                Data = html,
                Success = true
            };
        }
    }
}