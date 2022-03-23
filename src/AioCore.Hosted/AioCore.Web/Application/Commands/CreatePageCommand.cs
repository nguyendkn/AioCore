using AioCore.Redis.OM;
using AioCore.Redis.OM.Searching;
using AioCore.Web.Domain;
using AioCore.Web.Domain.AggregateModels.PageAggregate;
using MediatR;

namespace AioCore.Web.Application.Commands;

public class CreatePageCommand : IRequest<Page>
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    internal class Handler : IRequestHandler<CreatePageCommand, Page>
    {
        private readonly AioCoreContext _context;

        public Handler(AioCoreContext context)
        {
            _context = context;
        }

        public async Task<Page> Handle(CreatePageCommand request, CancellationToken cancellationToken)
        {
            return await _context.Pages.AddAsync(new Page(request.Name, request.Description));
        }
    }
}