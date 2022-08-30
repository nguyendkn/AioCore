using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Write.SettingCommands.TenantCommands;

public class SubmitTenantDomainCommand : SettingTenantDomain, IRequest<Response<SettingTenantDomain>>
{
    public SubmitTenantDomainCommand(Guid tenantId, string domain)
    {
        TenantId = tenantId;
        Domain = domain;
    }

    internal class Handler : IRequestHandler<SubmitTenantDomainCommand, Response<SettingTenantDomain>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<SettingTenantDomain>> Handle(SubmitTenantDomainCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id.Equals(Guid.Empty))
            {
                var tenantDomainEntityEntry = await _context.TenantDomains.AddAsync(request, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                var settingTenantDomain = tenantDomainEntityEntry.Entity;
                return new Response<SettingTenantDomain>
                {
                    Data = settingTenantDomain,
                    Message = Messages.CreateDataSuccessful,
                    Success = true
                };
            }
            else
            {
                var tenantDomain = await _context.TenantDomains.FirstOrDefaultAsync(
                    x => x.Id.Equals(request.Id), cancellationToken);
                if (tenantDomain is null)
                    return new Response<SettingTenantDomain>
                    {
                        Message = Messages.DataNotFound,
                        Success = false
                    };
                tenantDomain.Update(request.Domain);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<SettingTenantDomain>
                {
                    Data = tenantDomain,
                    Message = Messages.UpdateDataSuccessful,
                    Success = true
                };
            }
        }
    }
}