using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;

namespace AioCore.Write.SettingCommands.FormCommands;

public class SubmitFormCommand : SettingForm, IRequest<Response<SettingForm>>
{
    public SubmitFormCommand(Guid entityId, string name)
    {
        EntityId = entityId;
        Name = name;
        CreatedAt = DateTime.Now;
        ModifiedAt = DateTime.Now;
    }
    
    internal class Handler : IRequestHandler<SubmitFormCommand, Response<SettingForm>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<SettingForm>> Handle(SubmitFormCommand request, CancellationToken cancellationToken)
        {
            var entityEntry = await _context.Forms.AddAsync(request, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response<SettingForm>
            {
                Data = entityEntry.Entity,
                Message = Messages.CreateDataSuccessful,
                Success = true
            };
        }
    }
}