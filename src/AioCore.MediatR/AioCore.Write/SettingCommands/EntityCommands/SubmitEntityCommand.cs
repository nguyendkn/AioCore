using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.SeedWorks;
using MediatR;

namespace AioCore.Write.SettingCommands.EntityCommands;

public class SubmitEntityCommand : SettingEntity, IRequest<Response<SettingEntity>>
{
    internal class Handler : IRequestHandler<SubmitEntityCommand, Response<SettingEntity>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public Task<Response<SettingEntity>> Handle(SubmitEntityCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}