using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using MediatR;

namespace AioCore.Write.SettingCommands.TenantCommands;

public class SubmitTenantCommand : SettingTenant, IRequest<Response<SettingTenant>>
{
    internal class Handler : IRequestHandler<SubmitTenantCommand, Response<SettingTenant>>
    {
        private readonly AppSettings _appSettings;
        private readonly SettingsContext _settingsContext;

        public Handler(SettingsContext settingsContext, AppSettings appSettings)
        {
            _settingsContext = settingsContext;
            _appSettings = appSettings;
        }

        public async Task<Response<SettingTenant>> Handle(SubmitTenantCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id.Equals(Guid.Empty))
            {
                var tenantEntityEntry = await _settingsContext.Tenants.AddAsync(request, cancellationToken);
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