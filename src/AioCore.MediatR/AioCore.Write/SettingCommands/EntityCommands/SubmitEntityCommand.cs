using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;

namespace AioCore.Write.SettingCommands.EntityCommands;

public class SubmitEntityCommand : SettingEntity, IRequest<Response<SettingEntity>>
{
    public SubmitEntityCommand(string name)
    {
        Name = name;
        CreatedAt = DateTime.Now;
        ModifiedAt = DateTime.Now;
    }

    internal class Handler : IRequestHandler<SubmitEntityCommand, Response<SettingEntity>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<SettingEntity>> Handle(SubmitEntityCommand request,
            CancellationToken cancellationToken)
        {
            var entityEntry = await _context.Entities.AddAsync(request, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response<SettingEntity>
            {
                Data = entityEntry.Entity,
                Message = Messages.CreateDataSuccessful,
                Success = true
            };
        }
    }
}