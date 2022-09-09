using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.SettingAggregate;
using AioCore.Services;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using AioCore.Shared.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.SettingQueries.CodeQueries;

public class ListCodeQuery : IRequest<Response<List<SettingCode>>>
{
    public Guid TenantId { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public ListCodeQuery(Guid tenantId, int page, int pageSize)
    {
        TenantId = tenantId;
        Page = page;
        PageSize = pageSize;
    }

    internal class Handler : IRequestHandler<ListCodeQuery, Response<List<SettingCode>>>
    {
        private readonly SettingsContext _context;
        private readonly UserClaimsValue _userClaims;
        private readonly IClientService _clientService;

        public Handler(SettingsContext context,
            AuthenticationStateProvider stateProvider,
            IClientService clientService)
        {
            _context = context;
            _clientService = clientService;
            _userClaims = stateProvider.UserClaims();
        }

        public async Task<Response<List<SettingCode>>> Handle(ListCodeQuery request,
            CancellationToken cancellationToken)
        {
            var codes = await _context.Codes
                .Include(x => x.Child)
                .Include(x => x.Parent)
                .Where(x => _userClaims.IsAdmin &&
                            (x.Tenant.Domain.Equals(_clientService.Host()) ||
                             x.TenantId.Equals(request.TenantId)))
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