using AioCore.Redis.OM;
using AioCore.Redis.OM.Aggregation;
using AioCore.Redis.OM.Searching;
using AioCore.Web.Domain.AggregateModels.PageAggregate;
using MediatR;

namespace AioCore.Web.Application.Queries;

public class GetPageQuery : IRequest<Page>
{
    public string Id { get; set; } = default!;

    internal class Handler : IRequestHandler<GetPageQuery, Page>
    {
        private readonly IRedisCollection<Page> _collection;
        private readonly RedisConnectionProvider _provider;

        public Handler(RedisConnectionProvider provider)
        {
            _provider = provider;
            _collection = provider.RedisCollection<Page>();
        }

        public async Task<Page> Handle(GetPageQuery request, CancellationToken cancellationToken)
        {
            return await _collection.FindByIdAsync(request.Id);
        }
    }
}