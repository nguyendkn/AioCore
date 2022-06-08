using MediatR;
using Settings.Layout.Application.Responses;

namespace Settings.Layout.Application.Commands;

public class CreateLayoutCommand : IRequest<CreateLayoutResponse>
{
    internal class Handler : IRequestHandler<CreateLayoutCommand, CreateLayoutResponse>
    {
        private readonly LayoutContext _context;

        public Handler(LayoutContext context)
        {
            _context = context;
        }

        public Task<CreateLayoutResponse> Handle(CreateLayoutCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}