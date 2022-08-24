using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.ListFormQueries;

public class ListFormAttributeQuery : IRequest<Response<List<SettingFormAttribute>>>
{
    public Guid FormId { get; set; }
    
    public int Page { get; set; }

    public int PageSize { get; set; }

    public ListFormAttributeQuery(Guid formId, int page, int pageSize)
    {
        FormId = formId;
        Page = page;
        PageSize = pageSize;
    }
    
    internal class Handler : IRequestHandler<ListFormAttributeQuery, Response<List<SettingFormAttribute>>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SettingFormAttribute>>> Handle(ListFormAttributeQuery request, CancellationToken cancellationToken)
        {
            var entities = await _context.FormAttributes
                .Include(x=>x.Attribute)
                .Where(x=>x.FormId.Equals(request.FormId))
                .OrderBy(x => x.Order)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new Response<List<SettingFormAttribute>>
            {
                Data = entities,
                Success = true
            };
        }
    }
}