using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.TenantQueries;

public class ListTenantGroupQuery : IRequest<Response<List<SettingTenantGroup>>>
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public ListTenantGroupQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    internal class Handler : IRequestHandler<ListTenantGroupQuery, Response<List<SettingTenantGroup>>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SettingTenantGroup>>> Handle(ListTenantGroupQuery request,
            CancellationToken cancellationToken)
        {
            var tenantGroups = await _context.TenantGroups
                .OrderBy(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new Response<List<SettingTenantGroup>>
            {
                Data = tenantGroups,
                Success = true
            };
        }
    }
}