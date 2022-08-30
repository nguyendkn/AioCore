using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.TenantQueries;

public class ListTenantQuery : IRequest<Response<List<SettingTenant>>>
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public ListTenantQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    internal class Handler : IRequestHandler<ListTenantQuery, Response<List<SettingTenant>>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SettingTenant>>> Handle(ListTenantQuery request,
            CancellationToken cancellationToken)
        {
            var tenants = await _context.Tenants
                .Include(x=>x.Domains)
                .Include(x=>x.Group)
                .OrderByDescending(x=>x.ModifiedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new Response<List<SettingTenant>>
            {
                Data = tenants,
                Success = true
            };
        }
    }
}