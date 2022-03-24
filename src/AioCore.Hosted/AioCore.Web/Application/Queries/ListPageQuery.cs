using AioCore.Mongo.OM.MongoCore;
using AioCore.Web.Domain;
using MediatR;
using AioCore.Web.Domain.AggregateModels.PageAggregate;
using MongoDB.Driver;

namespace AioCore.Web.Application.Queries;

public class ListPageQuery : IRequest<List<Page>>
{
    public int Page { get; set; } = 0;

    public int PageSize { get; set; } = 20;

    internal class Handler : IRequestHandler<ListPageQuery, List<Page>>
    {
        private readonly AioCoreContext _context;

        public Handler(AioCoreContext context)
        {
            _context = context;
        }

        public async Task<List<Page>> Handle(ListPageQuery request, CancellationToken cancellationToken)
        {
            return await _context.Pages.Where(x => true).Skip(request.Page)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}