using MediatR;
using Settings.Setup.Application.Responses;

namespace Settings.Setup.Application.Queries;

public class BeginSetupQuery : IRequest<BeginSetupResponse>
{
    internal class Handler : IRequestHandler<BeginSetupQuery, BeginSetupResponse>
    {
        public Task<BeginSetupResponse> Handle(BeginSetupQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}