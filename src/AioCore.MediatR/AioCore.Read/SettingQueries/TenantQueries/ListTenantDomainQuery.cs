using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.TenantQueries;

public class ListTenantDomainQuery : IRequest<Response<List<SettingTenantDomain>>>
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public Guid TenantId { get; set; }

    public ListTenantDomainQuery(int page, int pageSize, Guid tenantId)
    {
        Page = page;
        PageSize = pageSize;
        TenantId = tenantId;
    }

    internal class Handler : IRequestHandler<ListTenantDomainQuery, Response<List<SettingTenantDomain>>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SettingTenantDomain>>> Handle(ListTenantDomainQuery request,
            CancellationToken cancellationToken)
        {
            var tenantGroups = await _context.TenantDomains
                .Where(x => x.TenantId.Equals(request.TenantId))
                .OrderBy(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new Response<List<SettingTenantDomain>>
            {
                Data = tenantGroups,
                Success = true
            };
        }
    }
}