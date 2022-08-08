using AioCore.Domain.IdentityAggregate;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AioCore.Read.IdentityQueries.RoleQueries;

public class ListRoleQuery : IRequest<Response<List<RoleResponse>>>
{
    internal class Handler : IRequestHandler<ListRoleQuery, Response<List<RoleResponse>>>
    {
        private readonly RoleManager<Role> _roleManager;

        public Handler(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Response<List<RoleResponse>>> Handle(ListRoleQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);
            var rolesResponse = BuildTree(null, roles.To<List<RoleResponse>>());
            return new Response<List<RoleResponse>>
            {
                Data = rolesResponse,
                Success = true
            };
        }

        private static List<RoleResponse> BuildTree(Guid? parentId, List<RoleResponse> source)
        {
            return source.Where(item =>
                (parentId == null && (item.ParentId == Guid.Empty)) ||
                item.ParentId == parentId).OrderBy(x => x.Index).Select(feature => new RoleResponse
            {
                Id = feature.Id,
                Name = feature.Name,
                Index = feature.Index,
                ParentId = feature.ParentId,
                Children = BuildTree(feature.Id, source)
            }).ToList();
        }
    }
}