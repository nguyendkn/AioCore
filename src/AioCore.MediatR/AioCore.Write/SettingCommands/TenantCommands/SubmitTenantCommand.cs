using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;

namespace AioCore.Write.SettingCommands.TenantCommands;

public class SubmitTenantCommand : SettingTenant, IRequest<Response<SettingTenant>>
{
    internal class Handler : IRequestHandler<SubmitTenantCommand, Response<SettingTenant>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<SettingTenant>> Handle(SubmitTenantCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id.Equals(Guid.Empty))
            {
                var tenantEntityEntry = await _context.Tenants.AddAsync(request, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                var settingTenant = tenantEntityEntry.Entity;
                return new Response<SettingTenant>
                {
                    Data = settingTenant,
                    Message = Messages.CreateDataSuccessful,
                    Success = true
                };
            }
            else
            {
                var tenantEntityEntry = await _context.Tenants.AddAsync(request, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                var settingTenant = tenantEntityEntry.Entity;
                return new Response<SettingTenant>
                {
                    Data = settingTenant,
                    Message = Messages.CreateDataSuccessful,
                    Success = true
                };
            }
        }
    }
}