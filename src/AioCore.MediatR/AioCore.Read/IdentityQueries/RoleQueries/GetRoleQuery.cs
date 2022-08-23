using AioCore.Domain.IdentityAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.Extensions;
using AioCore.Shared.SeedWorks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AioCore.Read.IdentityQueries.RoleQueries;

public class GetRoleQuery : IRequest<Response<RoleResponse>>
{
    public Guid Id { get; set; }

    public GetRoleQuery(Guid id)
    {
        Id = id;
    }

    internal class Handler : IRequestHandler<GetRoleQuery, Response<RoleResponse>>
    {
        private readonly RoleManager<Role> _roleManager;

        public Handler(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Response<RoleResponse>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (role is null)
                return new Response<RoleResponse>
                {
                    Message = Messages.DataNotFound,
                    Success = false
                };
            return new Response<RoleResponse>
            {
                Data = role.To<RoleResponse>(),
                Success = true
            };
        }
    }
}