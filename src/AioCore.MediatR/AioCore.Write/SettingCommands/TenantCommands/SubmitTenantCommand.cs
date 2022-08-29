using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AIoCore.Migrations.Migrations;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Write.SettingCommands.TenantCommands;

public class SubmitTenantCommand : SettingTenant, IRequest<Response<SettingTenant>>
{
    internal class Handler : IRequestHandler<SubmitTenantCommand, Response<SettingTenant>>
    {
        private readonly SettingsContext _settingsContext;
        private readonly DynamicContext _dynamicContext;

        public Handler(SettingsContext settingsContext, DynamicContext dynamicContext)
        {
            _settingsContext = settingsContext;
            _dynamicContext = dynamicContext;
        }

        public async Task<Response<SettingTenant>> Handle(SubmitTenantCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id.Equals(Guid.Empty))
            {
                var tenantEntityEntry = await _settingsContext.Tenants.AddAsync(request, cancellationToken);
                await _settingsContext.SaveChangesAsync(cancellationToken);
                DynamicContext.Schema = "aioc-" + tenantEntityEntry.Entity?.Id;
                await _dynamicContext.Database.MigrateAsync(cancellationToken);

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
                // TODO: Update tenant here
                return default!;
            }
        }
    }
}