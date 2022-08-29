using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Write.SettingCommands.TenantCommands;

public class SubmitTenantGroupCommand : SettingTenantGroup, IRequest<Response<SettingTenantGroup>>
{
    public SubmitTenantGroupCommand(string name)
    {
        Name = name;
    }

    internal class Handler : IRequestHandler<SubmitTenantGroupCommand, Response<SettingTenantGroup>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<SettingTenantGroup>> Handle(SubmitTenantGroupCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id.Equals(Guid.Empty))
            {
                var tenantGroupEntityEntry = await _context.TenantGroups.AddAsync(request, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                var settingTenantGroup = tenantGroupEntityEntry.Entity;
                return new Response<SettingTenantGroup>
                {
                    Data = settingTenantGroup,
                    Message = Messages.CreateDataSuccessful,
                    Success = true
                };
            }
            else
            {
                var tenantGroup = await _context.TenantGroups.FirstOrDefaultAsync(
                    x => x.Id.Equals(request.Id), cancellationToken);
                if (tenantGroup is null)
                    return new Response<SettingTenantGroup>
                    {
                        Message = Messages.DataNotFound,
                        Success = false
                    };
                tenantGroup.Update(request.Name);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<SettingTenantGroup>
                {
                    Data = tenantGroup,
                    Message = Messages.CreateDataSuccessful,
                    Success = true
                };
            }
        }
    }
}