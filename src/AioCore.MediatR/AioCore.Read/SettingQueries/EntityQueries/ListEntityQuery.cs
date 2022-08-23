using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Read.SettingQueries.TenantQueries;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.EntityQueries;

public class ListEntityQuery : IRequest<Response<List<SettingEntity>>>
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public ListEntityQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    internal class Handler : IRequestHandler<ListEntityQuery, Response<List<SettingEntity>>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SettingEntity>>> Handle(ListEntityQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _context.Entities
                .OrderByDescending(x => x.ModifiedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new Response<List<SettingEntity>>
            {
                Data = entities,
                Success = true
            };
        }
    }
}