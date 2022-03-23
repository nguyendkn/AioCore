using AioCore.Web.Domain;
using AioCore.Web.Domain.AggregateModels.PageAggregate;
using MediatR;

namespace AioCore.Web.Application.Queries;

public class GetPageQuery : IRequest<Page>
{
    public string Id { get; set; } = default!;

    internal class Handler : IRequestHandler<GetPageQuery, Page>
    {
        private readonly AioCoreContext _context;

        public Handler(AioCoreContext context)
        {
            _context = context;
        }

        public async Task<Page> Handle(GetPageQuery request, CancellationToken cancellationToken)
        {
            return await _context.Pages.FindAsync(request.Id);
        }
    }
}