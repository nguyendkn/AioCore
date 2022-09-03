using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.ListFormQueries;

public class ListFormQuery : IRequest<Response<List<SettingForm>>>
{
    public Guid TenantId { get; set; }
    
    public int Page { get; set; }

    public int PageSize { get; set; }

    public ListFormQuery(Guid tenantId, int page, int pageSize)
    {
        TenantId = tenantId;
        Page = page;
        PageSize = pageSize;
    }

    internal class Handler : IRequestHandler<ListFormQuery, Response<List<SettingForm>>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SettingForm>>> Handle(ListFormQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _context.Forms
                .Include(x => x.Entity)
                .OrderByDescending(x => x.ModifiedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new Response<List<SettingForm>>
            {
                Data = entities,
                Success = true
            };
        }
    }
}