using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.TenantQueries;

public class GetTenantQuery : IRequest<Response<SettingTenant>>
{
    public Guid Id { get; set; }

    public GetTenantQuery(Guid id)
    {
        Id = id;
    }

    internal class Handler : IRequestHandler<GetTenantQuery, Response<SettingTenant>>
    {
        private readonly SettingsContext _settingsContext;

        public Handler(SettingsContext settingsContext)
        {
            _settingsContext = settingsContext;
        }

        public async Task<Response<SettingTenant>> Handle(GetTenantQuery request, CancellationToken cancellationToken)
        {
            var tenant = await _settingsContext.Tenants.FirstOrDefaultAsync(
                x => x.Id.Equals(request.Id), cancellationToken);
            if (tenant is null) return new Response<SettingTenant>
            {
                Message = Messages.DataNotFound,
                Success = false
            };
            return new Response<SettingTenant>
            {
                Data = tenant,
                Success = true
            };
        }
    }
}