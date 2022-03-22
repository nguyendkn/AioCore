using AioCore.Redis.OM;
using AioCore.Redis.OM.Searching;
using AioCore.Web.Domain;
using AioCore.Web.Domain.AggregateModels.PageAggregate;
using MediatR;

namespace AioCore.Web.Application.Commands;

public class CreatePageCommand : IRequest<string>
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    internal class Handler : IRequestHandler<CreatePageCommand, string>
    {
        private readonly IRedisCollection<Page> _collection;

        public Handler(RedisConnectionProvider provider)
        {
            _collection = provider.RedisCollection<Page>();
        }

        public async Task<string> Handle(CreatePageCommand request, CancellationToken cancellationToken)
        {
            return await _collection.InsertAsync(new Page(request.Name, request.Description)).ConfigureAwait(false);
        }
    }
}