using AioCore.Web.Domain;
using AioCore.Web.Domain.AggregateModels.PageAggregate;
using MediatR;

namespace AioCore.Web.Application.Commands;

public class CreatePageCommand : IRequest<bool>
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    internal class Handler : IRequestHandler<CreatePageCommand, bool>
    {
        private readonly AioCoreContext _context;

        public Handler(AioCoreContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreatePageCommand request, CancellationToken cancellationToken)
        {
            await _context.Pages.AddAsync(new Page
            {
                Name = request.Name,
                Description = request.Description
            });
            return await Task.FromResult(true);
        }
    }
}