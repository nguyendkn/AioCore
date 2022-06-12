using MediatR;
using Settings.Setup.Application.Responses;

namespace Settings.Setup.Application.Commands;

public class StartSetupCommand : IRequest<StartSetupResponse>
{
    internal class Handler : IRequestHandler<StartSetupCommand, StartSetupResponse>
    {
        public Task<StartSetupResponse> Handle(StartSetupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}