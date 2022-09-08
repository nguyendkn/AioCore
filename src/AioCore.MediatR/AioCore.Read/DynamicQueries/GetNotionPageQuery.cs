using AioCore.Services.NotionService;
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

        public Task<Response<string>> Handle(GetNotionPageQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}