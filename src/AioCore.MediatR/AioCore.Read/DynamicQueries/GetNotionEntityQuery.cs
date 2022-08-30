using AioCore.Notion;
using AioCore.Shared.SeedWorks;
using MediatR;

namespace AioCore.Read.DynamicQueries;

public class GetNotionEntityQuery : IRequest<Response<Dictionary<string, object>>>
{
    public string Database { get; set; } = default!;
    
    public GetNotionEntityQuery(string database)
    {
        Database = database;
    }

    internal class Handler : IRequestHandler<GetNotionEntityQuery, Response<Dictionary<string, object>>>
    {
        private readonly INotionClient _notionClient;

        public Handler(INotionClient notionClient)
        {
            _notionClient = notionClient;
        }

        public async Task<Response<Dictionary<string, object>>> Handle(GetNotionEntityQuery request,
            CancellationToken cancellationToken)
        {
            var database = await _notionClient.QueryAsync<Dictionary<string, object>>(request.Database);
            throw new NotImplementedException();
        }
    }
}