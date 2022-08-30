using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AIoCore.Migrations.Migrations;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AioCore.Write.SettingCommands.TenantCommands;

public class SubmitTenantCommand : SettingTenant, IRequest<Response<SettingTenant>>
{
    internal class Handler : IRequestHandler<SubmitTenantCommand, Response<SettingTenant>>
    {
        private readonly AppSettings _appSettings;
        private readonly SettingsContext _settingsContext;
        private readonly IServiceProvider _serviceProvider;

        public Handler(SettingsContext settingsContext, 
            IServiceProvider serviceProvider, AppSettings appSettings)
        {
            _settingsContext = settingsContext;
            _serviceProvider = serviceProvider;
            _appSettings = appSettings;
        }

        public async Task<Response<SettingTenant>> Handle(SubmitTenantCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id.Equals(Guid.Empty))
            {
                var tenantEntityEntry = await _settingsContext.Tenants.AddAsync(request, cancellationToken);

                var schema = "aioc-" + tenantEntityEntry.Entity?.Id;
                await using (var dynamicContext = DynamicContext.GetContext(_appSettings, schema))
                    await dynamicContext.Database.MigrateAsync(cancellationToken);
                
                await _settingsContext.SaveChangesAsync(cancellationToken);
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