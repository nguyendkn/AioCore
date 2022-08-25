using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.CodeQueries;

public class ListCodeQuery : IRequest<Response<List<SettingCode>>>
{
    public int Page { get; set; }

    public int PageSize { get; set; }
    
    public ListCodeQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
    
    internal class Handler : IRequestHandler<ListCodeQuery, Response<List<SettingCode>>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SettingCode>>> Handle(ListCodeQuery request,
            CancellationToken cancellationToken)
        {
            var codes = await _context.Codes
                .Include(x=>x.Child)
                .Include(x=>x.Parent)
                .OrderBy(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new Response<List<SettingCode>>
            {
                Data = codes,
                Success = true
            };
        }
    }
}