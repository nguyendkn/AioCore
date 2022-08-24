using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.AttributeCommands;

public class ListAttributeQuery : IRequest<Response<List<SettingAttribute>>>
{
    public Guid EntityId { get; set; }
    
    public int Page { get; set; }

    public int PageSize { get; set; }

    public ListAttributeQuery(Guid entityId, int page, int pageSize)
    {
        EntityId = entityId;
        Page = page;
        PageSize = pageSize;
    }

    internal class Handler : IRequestHandler<ListAttributeQuery, Response<List<SettingAttribute>>>
    {
        private readonly SettingsContext _context;

        public Handler(SettingsContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SettingAttribute>>> Handle(ListAttributeQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _context.Attributes
                .Where(x=>x.EntityId.Equals(request.EntityId))
                .OrderBy(x => x.Order)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return new Response<List<SettingAttribute>>
            {
                Data = entities,
                Success = true
            };
        }
    }
}